using System;
using System.Collections.Generic;
using TenMaker.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace TenMaker.RankMode
{
    public class GridManager : MonoBehaviour
    {
        [field: SerializeField] public GridSystem GridSystem { get; private set; }
        [field: SerializeField] public RegionSumCalculator RegionSumCalculator { get; private set; }
        [field: SerializeField] public ValidRegionProcessor ValidRegionProcessor { get; private set; }

        public void Setup(int col, int row)
        {
            GridSystem.Setup(col, row);
            RegionSumCalculator.Setup(GridSystem);
            ValidRegionProcessor.Setup(GridSystem, RegionSumCalculator);
        }
        public void CreateNewGrid()
        {
            const int maxTryCount = 100;
            var tryCount = 0;
            do
            {
                GridSystem.CreateGrid();
                RegionSumCalculator.ComputePrefixSum();
                ValidRegionProcessor.ComputeValidRegions();
            } while (!ValidRegionProcessor.IsValidRegionExists() && tryCount++ < maxTryCount);
        }

        public void SetVisibility(bool value)
        {
            GridSystem.SetVisibility(value);
        }

        public Cell GetCell(Vector2 worldPosition) => GridSystem.GetCell(worldPosition);

        public Cell GetNearestCell(Vector2 worldPosition) => GridSystem.GetNearestCell(worldPosition);

        public int TryClearSelection(Cell startCell, Cell endCell, List<Cell> clearedCells)
        {
            if (ValidRegionProcessor.IsValidRegion(startCell, endCell) is false) return 0;

            var clearedCount = GridSystem.ClearRegion(startCell, endCell, clearedCells);

            if (GridSystem.ValueCellCount is not 0)
            {
                RegionSumCalculator.ComputePrefixSum();
                ValidRegionProcessor.ComputeValidRegions();
            }
            else
            {
                CreateNewGrid();
            }

            return clearedCount;
        }
    }
}