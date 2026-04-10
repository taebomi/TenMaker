using System;
using System.Collections.Generic;
using UnityEngine;

namespace TenMaker.Core
{
    public class ValidRegionProcessor : MonoBehaviour
    {
        public const int REGION_SUM = 10;

        private GridSystem _gridSystem;
        private RegionSumCalculator _regionSumCalculator;

        public List<Region> ValidRegions { get; private set; }

        private void Awake()
        {
            ValidRegions = new List<Region>();
        }


        public void Setup(GridSystem gridSystem, RegionSumCalculator regionSumCalculator)
        {
            _gridSystem = gridSystem;
            _regionSumCalculator = regionSumCalculator;
        }

        public bool IsValidRegionExists()
        {
            return ValidRegions.Count > 0;
        }


        /// <summary>
        /// 규칙을 만족하는 영역을 찾아 캐싱
        /// </summary>
        // ReSharper disable once CognitiveComplexity
        public void ComputeValidRegions()
        {
            ValidRegions.Clear();

            var rowCount = _gridSystem.RowCount;
            var colCount = _gridSystem.ColCount;

            for (var startRow = 0; startRow < rowCount; startRow++)
            {
                for (var startCol = 0; startCol < colCount; startCol++)
                {
                    for (var endRow = startRow; endRow < rowCount; endRow++)
                    {
                        for (var endCol = startCol; endCol < colCount; endCol++)
                        {
                            if (IsValidAndMinimumRegion(startRow, startCol, endRow, endCol) is false) continue;

                            ValidRegions.Add(new Region(_gridSystem[startRow, startCol], _gridSystem[endRow, endCol]));
                        }
                    }
                }
            }
        }

        public bool IsValidRegion(Cell startCell, Cell endCell)
        {
            return _regionSumCalculator.GetRegionSum(startCell, endCell) == REGION_SUM;
        }

        public bool IsValidRegion(int minRow, int minCol, int maxRow, int maxCol)
        {
            return _regionSumCalculator.GetRegionSum(minRow, minCol, maxRow, maxCol) == REGION_SUM;
        }

        private bool IsValidAndMinimumRegion(int minRow, int minCol, int maxRow, int maxCol)
        {
            if (IsValidRegion(minRow, minCol, maxRow, maxCol) is false)
            {
                return false;
            }

            if (minRow < maxRow &&
                IsValidRegion(minRow + 1, minCol, maxRow, maxCol) ||
                IsValidRegion(minRow, minCol, maxRow - 1, maxCol))
            {
                return false;
            }


            if (minCol < maxCol &&
                IsValidRegion(minRow, minCol + 1, maxRow, maxCol) ||
                IsValidRegion(minRow, minCol, maxRow, maxCol - 1))
            {
                return false;
            }

            return true;
        }
    }
}