using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TenMaker.Core.Scene;
using TenMaker.Utility.Core;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Matchmaker;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TenMaker.Services.UGS.Multiplay
{
    [DefaultExecutionOrder(-2)]
    public class MultiplayManager : MonoBehaviour, ITMMultiplayService
    {
        private const string CONNECTION_TYPE = "dtls";

        [Title("Components")]
        [field: SerializeField] public NetworkManager NetworkManager { get; private set; }
        [field: SerializeField] public UnityTransport Transport { get; private set; }

        public IRelayService RelayService { get; private set; }
        public IMatchmakerService MatchmakerService { get; private set; }
        public ILobbyService LobbyService { get; private set; }

        public bool IsServer => NetworkManager != null && NetworkManager.IsServer;
        public bool IsClient => NetworkManager != null && NetworkManager.IsClient;

        public event Action OnAllPlayersConnected;

        public void Initialize()
        {
            RelayService = Unity.Services.Relay.RelayService.Instance;
            MatchmakerService = Unity.Services.Matchmaker.MatchmakerService.Instance;
            LobbyService = Unity.Services.Lobbies.LobbyService.Instance;
        }


        public async UniTask<Result<CreateRoomPayload>> CreateRoomAsync(CancellationToken ct)
        {
            Allocation allocation;
            try
            {
                allocation = await RelayService.CreateAllocationAsync(MultiplayContext.MAX_PLAYERS);
                ct.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException)
            {
                return Result<CreateRoomPayload>.Fail(ErrorCode.CANCELLED);
            }
            catch
            {
                return Result<CreateRoomPayload>.Fail(ErrorCode.UNKNOWN);
            }

            string joinCode;
            try
            {
                joinCode = await RelayService.GetJoinCodeAsync(allocation.AllocationId);
                ct.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException)
            {
                return Result<CreateRoomPayload>.Fail(ErrorCode.CANCELLED);
            }
            catch
            {
                return Result<CreateRoomPayload>.Fail(ErrorCode.UNKNOWN);
            }

            var relayServerData = allocation.ToRelayServerData(CONNECTION_TYPE);
            Transport.SetRelayServerData(relayServerData);
            NetworkManager.StartHost();
            RegisterNetworkEvents();

            return Result<CreateRoomPayload>.Success(new CreateRoomPayload(joinCode));
        }


        public async UniTask<Result> JoinRoomAsync(string joinCode, CancellationToken ct)
        {
            JoinAllocation allocation;
            try
            {
                allocation = await RelayService.JoinAllocationAsync(joinCode: joinCode);
                ct.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException)
            {
                return Result.Fail(ErrorCode.CANCELLED);
            }
            catch
            {
                return Result.Fail(ErrorCode.UNKNOWN);
            }

            var relayServerData = allocation.ToRelayServerData(CONNECTION_TYPE);
            Transport.SetRelayServerData(relayServerData);
            NetworkManager.StartClient();
            RegisterNetworkEvents();

            return Result.Success();
        }
        public async UniTask LeaveRoomAsync()
        {
            UnregisterNetworkEvents();

            if (NetworkManager.IsConnectedClient || NetworkManager.IsServer || NetworkManager.IsHost)
                NetworkManager.Shutdown();

            await UniTask.WaitUntil(
                () => !NetworkManager.ShutdownInProgress,
                cancellationToken: destroyCancellationToken);
        }

        public void StartGame()
        {
            if (!IsServer) return;
            NetworkManager.SceneManager.LoadScene(SceneNames.DUEL_MODE, LoadSceneMode.Single);
        }


        private void RegisterNetworkEvents()
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.SceneManager.OnLoad += OnNetworkSceneLoad;
        }

        private void UnregisterNetworkEvents()
        {
            if (NetworkManager == null) return;
            NetworkManager.OnClientConnectedCallback -= OnClientConnected;
            if (NetworkManager.SceneManager != null)
                NetworkManager.SceneManager.OnLoad -= OnNetworkSceneLoad;
        }

        private void OnClientConnected(ulong clientId)
        {
            if (!IsServer) return;
            if (NetworkManager.ConnectedClientsIds.Count >= MultiplayContext.MAX_PLAYERS)
                OnAllPlayersConnected?.Invoke();
        }

        private void OnNetworkSceneLoad(ulong clientId, string sceneName, LoadSceneMode loadSceneMode, AsyncOperation asyncOperation)
        {
            if (clientId != NetworkManager.LocalClientId) return;

            TMSceneService.Instance.LoadSceneWithMultiplay(sceneName, asyncOperation);
            UnregisterNetworkEvents();
        }
    }
}
