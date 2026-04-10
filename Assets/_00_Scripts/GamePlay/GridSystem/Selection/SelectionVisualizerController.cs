using System.Collections.Generic;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.Player.Multiplay;
using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public class SelectionVisualizerController : MonoBehaviour
    {
        // modules
        public PlayerSelectionVisualizer LocalVisualizer { get; private set; }
        public Dictionary<ulong, PlayerSelectionVisualizer> RemoteVisualizers { get; private set; } = new();
        
        // references
        private GridVisualizer _gridVisualizer;

        // Initialization
        public void Initialize(GridVisualizer visualizer, PlayersContext playerContext)
        {
            _gridVisualizer = visualizer;

            var tr = transform;
            foreach (var player in playerContext.AllPlayers.Values)
            {
                var selectionVisualizerPrefab = player.CustomizationSystem.GetSelectionVisualizer();
                var selectionVisualizer = Instantiate(selectionVisualizerPrefab, tr.position,
                    Quaternion.identity, tr);
                RemoteVisualizers.Add(player.OwnerClientId, selectionVisualizer);
                if (player.IsOwner)
                {
                    LocalVisualizer = selectionVisualizer;
                    selectionVisualizer.Initialize(true);
                }
                else
                {
                    selectionVisualizer.Initialize(false);
                }
            }

            if (LocalVisualizer == null)
            {
                TBMLog.HeaderError("Local Visualizer is null");
                return;
            }
        }

        // public Methods
        public void UpdateLocalSelection(Region selectedRegion)
        {
            var (centerPos, size) = _gridVisualizer.ConvertToArea(selectedRegion);
            LocalVisualizer.UpdateVisual(centerPos, size);
        }

        public void HideLocalSelection()
        {
            LocalVisualizer.Hide();
        }

        public void UpdateRemoteSelection(ulong clientId, Region selectedRegion)
        {
            if (RemoteVisualizers.TryGetValue(clientId, out var remoteVisualizer) is false)
            {
                TBMLog.HeaderWarning($"{clientId} does not exist");
                return;
            }

            var (centerPos, size) = _gridVisualizer.ConvertToArea(selectedRegion);
            remoteVisualizer.UpdateVisual(centerPos, size);
        }

        public void HideRemoteSelection(ulong clientId)
        {
            if (RemoteVisualizers.TryGetValue(clientId, out var remoteVisualizer) is false)
            {
                TBMLog.HeaderWarning($"{clientId} does not exist");
                return;
            }

            remoteVisualizer.Hide();
        }
    }
}