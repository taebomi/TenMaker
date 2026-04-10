using Sirenix.OdinInspector;
using TenMaker.Gameplay.CameraSystem;
using TenMaker.Gameplay.Combo;
using TenMaker.Gameplay.Player.Multiplay;
using TenMaker.Gameplay.Score.Multiplay;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Gameplay.Multiplay
{
    public class RegionSelectionHandler : NetworkBehaviour
    {
        [TitleGroup("References")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private GridController gridController;
        [SerializeField] private ClearEffectController clearEffectController;
        [SerializeField] private ScoreSystem scoreSystem;
        [SerializeField] private ComboController comboController;

        [TitleGroup("Data")]
        [SerializeField] private ComboCameraShakeDataSO comboCameraShakeData;

        public void Initialize(PlayersContext playerContext)
        {
            foreach (var playerObject in playerContext.AllPlayers.Values)
            {
                clearEffectController.Register(playerObject);
            }
        }

        // Server
        public void UpdateSelectionServer(ulong clientId, Region selectedRegion)
        {
            var exceptSender = RpcTarget.Not(clientId, RpcTargetUse.Temp);
            gridController.UpdateRemoteSelectionClientRpc(clientId, selectedRegion, exceptSender);
        }

        public void FinishSelectionServer(ulong clientId, Region selectedRegion)
        {
            var exceptSender = RpcTarget.Not(clientId, RpcTargetUse.Temp);
            gridController.HideRemoteSelectionClientRpc(clientId, exceptSender);

            var regionClearResult = gridController.ClearRegionServer(clientId, selectedRegion);
            if (regionClearResult == null) return;

            var clearResult = regionClearResult.Value;

            comboController.IncreaseComboServer(clientId);
            scoreSystem.UpdateScoreServer();

            var clearEvent =
                new ClearEventDTO(clientId, clearResult, comboController.GetCombo(clientId), scoreSystem.GetScoreDTO());
            HandleRegionClearedClientRpc(clearEvent);
        }

        [Rpc(SendTo.ClientsAndHost, RequireOwnership = true)]
        private void HandleRegionClearedClientRpc(ClearEventDTO clearEvent)
        {
            var clearedCellObjects = gridController.GetCellObjects(clearEvent.ClearedCellCoordinates);
            var clearEventContext = new ClearEventContext(clearEvent, clearedCellObjects);

            var isLocal = clearEvent.ClientId == NetworkManager.LocalClientId;

            gridController.ApplyRegionCleared(clearEventContext);
            if (isLocal)
            {
                clearEffectController.PlayLocalEffect(clearedCellObjects);
                cameraController.ShakeCamera(comboCameraShakeData.GetData(clearEvent.Combo));
            }
            else
            {
                clearEffectController.PlayRemoteEffect(clearEvent.ClientId, clearedCellObjects);
            }

            // combo
            var comboEffectPosition = gridController.GetRegionCenter(clearEvent.ClearedRegion);
            comboController.PlayComboEffect(clearEvent.Combo, comboEffectPosition);
            scoreSystem.UpdateScore(clearEvent.ScoreData);
        }

        // Client
        public void PerformLocalDrag(Region selectedRegion)
        {
            gridController.UpdateLocalSelection(selectedRegion);
        }

        public void FinishLocalDrag()
        {
            gridController.HideLocalSelection();
        }
    }
}