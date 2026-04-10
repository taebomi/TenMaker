using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.GridSystem;
using TenMaker.Gameplay.Player.Multiplay;
using Unity.Netcode;
using UnityEngine;
using Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;
using GridSystem_Multiplay_Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;
using Multiplay_Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;

namespace TenMaker.Gameplay.Multiplay
{
    public class GridController : NetworkBehaviour
    {
        [TitleGroup("Config"), SerializeField] private GridWeightedValueSetSO gridWeightedValueSet;
        [TitleGroup("Config"), SerializeField] private GridConfigSO gridConfigSO;

        // Server
        public GridSystem_Multiplay_Grid ServerGrid { get; private set; }
        public ValidRegionChecker ValidRegionChecker { get; private set; }
        private GridGenerator _gridGenerator;


        // Client Modules
        public GridSystem_Multiplay_Grid LocalGrid { get; private set; }
        [field: SerializeField] public GridView GridView { get; private set; }

        #region Initialize

        /// <summary>
        /// 서버측 초기화, 초기 Grid 생성
        /// </summary>
        /// <returns></returns>
        public void InitializeServer()
        {
            var configData = gridConfigSO.data;
            _gridGenerator = new GridGenerator(gridWeightedValueSet.items, configData.rowCount, configData.colCount);
            ServerGrid = new GridSystem_Multiplay_Grid(configData, _gridGenerator.GenerateValues());
            ValidRegionChecker = new ValidRegionChecker(ServerGrid);
        }

        public GridInitializationDTO GetInitializationData()
        {
            var cellInitializationData = new CellInitializationDTO[ServerGrid.Cells.Length];
            for (var i = 0; i < cellInitializationData.Length; i++)
            {
                cellInitializationData[i] = new CellInitializationDTO(ServerGrid[i].Value);
            }

            return new GridInitializationDTO(gridConfigSO.data, cellInitializationData);
        }

        public void Initialize(GridInitializationDTO data, PlayersContext playerContext)
        {
            LocalGrid = new GridSystem_Multiplay_Grid(data);
            GridView.InitializeClient(LocalGrid, playerContext);
        }

        #endregion

        public List<CellObject> GetCellObjects(Vector2Int[] coordinates)
        {
            return GridView.GetCellObjects(coordinates);
        }

        #region Selection

        // Visualization
        // Local Seleciton 처리
        public void UpdateLocalSelection(Region selectedRegion)
        {
            GridView.UpdateLocalSelection(selectedRegion);
        }

        public void HideLocalSelection()
        {
            GridView.HideLocalSelection();
        }

        // Remote Selection 처리
        [Rpc(SendTo.SpecifiedInParams, RequireOwnership = true)]
        public void UpdateRemoteSelectionClientRpc(ulong targetClientId, Region selectedRegion, RpcParams target)
        {
            GridView.UpdateRemoteSelection(targetClientId, selectedRegion);
        }

        [Rpc(SendTo.SpecifiedInParams, RequireOwnership = true)]
        public void HideRemoteSelectionClientRpc(ulong targetClientId, RpcParams target)
        {
            GridView.HideRemoteSelection(targetClientId);
        }

        // Clear
        public RegionClearResult? ClearRegionServer(ulong clearedClientId, Region region)
        {
            if (ValidRegionChecker.IsValidRegion(region) is false) return null;

            // 최소 유효 영역을 해당 플레이어 Clear 처리
            region = ValidRegionChecker.TrimMinimalValidRegion(region);
            var clearedCellCoordinates = ServerGrid.ClearRegion(clearedClientId, region);
            if (clearedCellCoordinates.Count == 0) return null;

            ValidRegionChecker.Compute();

            return new RegionClearResult(region, clearedCellCoordinates);
        }

        public void ApplyRegionCleared(ClearEventContext clearEvent)
        {
            LocalGrid.ApplyClearedRegion(clearEvent.ClientId, clearEvent.Region);
            GridView.ApplyClearedRegion(clearEvent);
        }

        #endregion

        public Vector2 GetRegionCenter(Region region)
        {
            return GridView.GetRegionCenter(region);
        }
    }
}