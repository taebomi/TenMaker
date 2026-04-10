using System;
using System.Collections.Generic;
using TenMaker.Core;
using TenMaker.Gameplay.GridSystem;

namespace TenMaker.Gameplay
{
    public class ValidRegionProcessor
    {
        public List<Region> ValidRegions { get; private set; } = new();

        private readonly RegionSumCalculator _regionSumCalculator;

        private readonly int _rowCount;
        private readonly int _colCount;

        public ValidRegionProcessor(RegionSumCalculator regionSumCalculator, GridConfigData gridConfig)
        {
            _regionSumCalculator = regionSumCalculator ??
                                   throw new ArgumentNullException($"Region Sum Calculator Cannot Be NULL");
            _rowCount = gridConfig.rowCount;
            _colCount = gridConfig.colCount;
        }

        public bool IsValidRegionExists()
        {
            return ValidRegions.Count > 0;
        }

        public bool IsValidRegion(Region region)
        {
            return _regionSumCalculator.GetRegionSum(region) == GameRule.TARGET_SUM;
        }

        public void ComputeValidRegions()
        {
            ValidRegions.Clear();

            for (var startRow = 0; startRow < _rowCount; startRow++)
            {
                for (var startCol = 0; startCol < _colCount; startCol++)
                {
                    for (var endRow = startRow; endRow < _rowCount; endRow++)
                    {
                        for (var endCol = startCol; endCol < _colCount; endCol++)
                        {
                            var region = new Region(startRow, startCol, endRow, endCol);
                            if (IsMinimumValidRegion(region) is false) continue;

                            ValidRegions.Add(region);
                        }
                    }
                }
            }
        }

        private bool IsMinimumValidRegion(Region region)
        {
            if (IsValidRegion(region) is false) return false;

            if (region.minRow < region.maxRow &&
                IsValidRegion(region.ShrinkFromBottom) || IsValidRegion(region.ShrinkFromTop))
            {
                return false;
            }

            if (region.minCol < region.maxCol &&
                IsValidRegion(region.ShrinkFromLeft) || IsValidRegion(region.ShrinkFromRight))
            {
                return false;
            }

            return true;
        }
    }
}