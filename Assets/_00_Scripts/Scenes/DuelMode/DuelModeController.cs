using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TenMaker.Services.UGS.Multiplay;
using TenMaker.Core.Scene;
using TenMaker.Gameplay;
using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.GridSystem;
using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.Player.Multiplay;
using TenMaker.Core.Input;
using TenMaker.Gameplay.Score.Multiplay;
using TenMaker.Gameplay.Score.Multiplay.Data;
using TenMaker.Gameplay.Timer;
using TenMaker.Scenes;
using TenMaker.Utility;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Scenes.DuelMode
{
    public class DuelModeController : NetworkSceneController
    {
        public override string SceneName => SceneNames.DUEL_MODE;

        // References
        [field: TitleGroup("References"), SerializeField]
        public Camera MainCamera { get; private set; }

        // Modules
        [field: TitleGroup("Modules"), SerializeField]
        public GridController GridController { get; private set; }
        [field: TitleGroup("Modules"), SerializeField]
        public RegionSelectionHandler RegionSelectionHandler { get; private set; }
        [field: TitleGroup("Modules"), SerializeField]
        public NetworkTimerSystem TimerSystem { get; private set; }
        [field: TitleGroup("Modules"), SerializeField]
        public ScoreSystem ScoreSystem { get; private set; }
        [field: TitleGroup("Modules"), SerializeField]
        public ComboController ComboController { get; private set; }

        // Prefabs
        [TitleGroup("Prefabs"), SerializeField]
        private GameObject playerObjectPrefab;

        // Config
        [TitleGroup("Config"), SerializeField] private GridConfigSO gridConfigSO;

        // UI
        [TitleGroup("UI"), SerializeField] private DuelResultUI resultUI;

        // Player Objects
        public PlayersContext PlayerContext { get; private set; }

        // Server Private
        private Dictionary<ulong, bool> PlayerInitializations { get; set; }

        protected override async UniTask InitializeSceneAsync()
        {
            if (!IsServer) return;

            var initializationData = await InitializeServerAsync(destroyCancellationToken);
            InitializeClientRpc(initializationData);
        }

        #region Server Initialization

        private async UniTask<NetworkInitializationData> InitializeServerAsync(CancellationToken ct)
        {
            // 모든 클라이언트가 씬에 로드될 때까지 대기
            await UniTask.WaitUntil(
                () => NetworkManager.ConnectedClientsIds.Count >= MultiplayContext.MAX_PLAYERS,
                cancellationToken: ct);

            var connectedClientIds = NetworkManager.ConnectedClientsIds;

            // 플레이어 오브젝트 스폰 후 완료 대기
            var playerObjects = CreatePlayerObjectsServer(connectedClientIds);
            await UniTask.WaitUntil(
                () => playerObjects.All(p => p.NetworkObject.IsSpawned),
                cancellationToken: ct);

            // 클라이언트 초기화 완료 추적용 딕셔너리
            PlayerInitializations = new Dictionary<ulong, bool>(connectedClientIds.Count);
            foreach (var clientId in connectedClientIds)
                PlayerInitializations.Add(clientId, false);

            // 서버 모듈 초기화
            GridController.InitializeServer();
            ComboController.InitializeServer(connectedClientIds);
            ScoreSystem.InitializeServer(connectedClientIds, GridController.ServerGrid);

            // 클라이언트로 보낼 초기화 데이터 생성
            var playerColorAssigner = new PlayerColorAssigner();
            var playerCustomizationData = new PlayerCustomizationData[playerObjects.Count];
            for (var i = 0; i < playerCustomizationData.Length; i++)
            {
                playerCustomizationData[i] = new PlayerCustomizationData
                {
                    ClientId = playerObjects[i].OwnerClientId,
                    Color = playerColorAssigner.GetRandomColor(),
                };
            }

            return new NetworkInitializationData
            {
                PlayerObjectReferences = playerObjects
                    .Select(p => new NetworkBehaviourReference(p))
                    .ToArray(),
                gridInitializationData = GridController.GetInitializationData(),
                PlayerCustomizationData = playerCustomizationData,
            };
        }

        /// <summary>
        /// 플레이어 오브젝트 생성 및 변수에 저장
        /// </summary>
        private List<NetworkBehaviour> CreatePlayerObjectsServer(IReadOnlyList<ulong> clientIds)
        {
            var playerObjects = new List<NetworkBehaviour>(clientIds.Count);
            foreach (var clientId in clientIds)
            {
                var playerObject = Instantiate(playerObjectPrefab, transform);
                var networkBehaviour = playerObject.GetComponent<NetworkBehaviour>();
                networkBehaviour.NetworkObject.SpawnAsPlayerObject(clientId, true);
                playerObjects.Add(networkBehaviour);
            }

            return playerObjects;
        }

        #endregion

        #region Client Initialization

        /// <summary>
        /// 서버 초기화 완료 이후 각 클라이언트 초기화
        /// </summary>
        /// <param name="initializationData"></param>
        [Rpc(SendTo.ClientsAndHost)]
        private void InitializeClientRpc(NetworkInitializationData initializationData)
        {
            InitializeClientAsync(initializationData, destroyCancellationToken).Forget();
        }

        private async UniTaskVoid InitializeClientAsync(NetworkInitializationData initializationData,
            CancellationToken ct)
        {
            // 모든 플레이어 커스터마이징 데이터 로드
            InitializePlayerContext(initializationData.PlayerObjectReferences);
            await LoadPlayerCustomizationAssets(initializationData.PlayerCustomizationData, ct);

            // Initialize
            TimerSystem.InitializeClient();
            GridController.Initialize(initializationData.gridInitializationData, PlayerContext);
            RegionSelectionHandler.Initialize(PlayerContext);
            ScoreSystem.Initialize(PlayerContext);
            foreach (var playerObject in PlayerContext.AllPlayers.Values)
            {
                await playerObject.InitializeAsync(this, ct);
            }

            NotifyClientInitializeFinishedServerRpc();
        }


        /// <summary>
        /// 서버로부터 받은 playerReference를 사용하여 PlayerContext 초기화
        /// </summary>
        /// <param name="playerReferences"></param>
        private void InitializePlayerContext(NetworkBehaviourReference[] playerReferences)
        {
            NetworkPlayerObject localPlayer = null;
            var remotePlayers = new Dictionary<ulong, NetworkPlayerObject>(playerReferences.Length - 1);

            foreach (var reference in playerReferences)
            {
                if (reference.TryGet(out NetworkPlayerObject playerObject) is false)
                {
                    TBMLog.HeaderError($"{reference} - NetworkPlayerObject Try Get Failed");
                    continue;
                }

                if (playerObject.IsOwner)
                {
                    localPlayer = playerObject;
                    continue;
                }

                if (remotePlayers.TryAdd(playerObject.OwnerClientId, playerObject) is false)
                {
                    TBMLog.HeaderError($"PlayerObject already exists for ClientId: {playerObject.OwnerClientId}");
                    continue;
                }
            }

            if (localPlayer == null)
            {
                TBMLog.HeaderError("Local PlayerObject not found");
                return;
            }

            PlayerContext = new PlayersContext(localPlayer, remotePlayers);
        }

        private async UniTask LoadPlayerCustomizationAssets(IEnumerable<PlayerCustomizationData> data,
            CancellationToken ct)
        {
            var tasks = data.Select(customizationData =>
            {
                if (PlayerContext.AllPlayers.TryGetValue(customizationData.ClientId, out var playerObject) is false)
                {
                    TBMLog.HeaderError($"PlayerObject not found for ClientId: {customizationData.ClientId}");
                    return UniTask.CompletedTask;
                }

                return playerObject.LoadCustomizationAssets(customizationData, ct);
            });

            await UniTask.WhenAll(tasks);
        }

        #endregion

        protected override async UniTask ProcessSceneAsync()
        {
            if (!IsServer) return;

            await WaitForClientInitializedServerAsync(destroyCancellationToken);
            StartGameServer();
        }

        /// <summary>
        /// 모든 클라이언트가 초기화 완료되기를 기다림
        /// </summary>
        /// <param name="ct"></param>
        private async UniTask WaitForClientInitializedServerAsync(CancellationToken ct)
        {
            bool AllClientsInitialized()
            {
                return PlayerInitializations.All(pair => pair.Value);
            }

            await UniTask.WaitUntil(AllClientsInitialized, cancellationToken: ct);
        }

        [Rpc(SendTo.Server)]
        private void NotifyClientInitializeFinishedServerRpc(RpcParams rpcParams = default)
        {
            var senderId = rpcParams.Receive.SenderClientId;
            if (PlayerInitializations.ContainsKey(senderId))
            {
                PlayerInitializations[senderId] = true;
            }
        }

        private void StartGameServer()
        {
            TimerSystem.StartTimerServer(120f); // todo 나중에 세팅 가능하도록
            StartGameClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost, InvokePermission = RpcInvokePermission.Owner)]
        private void StartGameClientRpc()
        {
            PlayerContext.LocalPlayer.Enable();
            BindGameplayEvents();
        }

        #region Events

        private void BindGameplayEvents()
        {
            TimerSystem.TimerFinished += OnTimerFinished;
        }

        private void UnbindGameplayEvents()
        {
            TimerSystem.TimerFinished -= OnTimerFinished;
        }

        private void OnTimerFinished()
        {
            UnbindGameplayEvents();

            if (!IsServer) return;

            ScoreSystem.UpdateScoreServer();
            EndGameClientRpc(ScoreSystem.GetScoreDTO());
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void EndGameClientRpc(ScoreDTO scoreDto)
        {
            PlayerContext.LocalPlayer.Disable();
            TMInputManager.Instance.Player.SwitchToUI();
            resultUI.Show(scoreDto, NetworkManager.LocalClientId);
        }

        #endregion
    }
}