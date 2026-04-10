using System.Collections.Generic;
using Sirenix.OdinInspector;
using TenMaker.Gameplay;
using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Player.Multiplay;
using Unity.Netcode;
using UnityEngine;
using Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;
using GridSystem_Multiplay_Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;
using Multiplay_Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;

namespace TenMaker.Gameplay.Multiplay
{
    public class GridView : NetworkBehaviour
    {
        // Modules
        [field: TitleGroup("Components"), SerializeField]
        public GridVisualizer GridVisualizer { get; private set; }
        [field: TitleGroup("Components"), SerializeField]
        public SelectionVisualizerController SelectionVisualizerController { get; private set; }

        //  todo : Valid Region Visualizer
        // public ValidRegionVisualizer ValidRegionVisualizer { get; private set; }

        public void InitializeClient(GridSystem_Multiplay_Grid grid, PlayersContext playerContext)
        {
            GridVisualizer.InitializeClient(grid, playerContext);
            SelectionVisualizerController.Initialize(GridVisualizer, playerContext);
        }

        #region Grid Visualizer

        public CellObject GetCellObject(Vector2 worldPosition)
        {
            return GridVisualizer.GetCellObject(worldPosition);
        }

        public CellObject GetNearestCellObject(Vector2 worldPosition)
        {
            return GridVisualizer.GetNearestCell(worldPosition);
        }

        public List<CellObject> GetCellObjects(Vector2Int[] coordinates)
        {
            return GridVisualizer.GetCellObjects(coordinates);
        }
        
        public void ApplyClearedRegion(ClearEventContext clearEvent)
        {
            GridVisualizer.ApplyClearedRegion(clearEvent);
        }

        public Vector2 GetRegionCenter(Region region)
        {
            return GridVisualizer.GetRegionCenter(region);
        }
        

        #endregion

        #region Selection Visualizer

        public void UpdateLocalSelection(Region selectedRegion)
        {
            SelectionVisualizerController.UpdateLocalSelection(selectedRegion);
        }

        public void HideLocalSelection()
        {
            SelectionVisualizerController.HideLocalSelection();
        }

        public void UpdateRemoteSelection(ulong targetClientId, Region selectedRegion)
        {
            SelectionVisualizerController.UpdateRemoteSelection(targetClientId, selectedRegion);
        }

        public void HideRemoteSelection(ulong targetClientId)
        {
            SelectionVisualizerController.HideRemoteSelection(targetClientId);
        }

        #endregion
    }
}