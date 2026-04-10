using System;
using System.Collections.Generic;
using System.Linq;
using TenMaker.Core;
using TenMaker.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TenMaker.Gameplay
{
    /// <summary>
    /// GameRule을 만족하는 랜덤한 값 생성
    /// </summary>
    public class GridGenerator
    {
        private readonly WeightedRandomPicker<int> _numberRandomPicker;
        private readonly int[,] _prefixSums;
        private readonly int _rowCount;
        private readonly int _colCount;


        public GridGenerator(IEnumerable<WeightedItem<int>> weightedNumbers, int rowCount, int colCount)
        {
            _numberRandomPicker = new WeightedRandomPicker<int>(weightedNumbers);
            _prefixSums = new int[rowCount + 1, colCount + 1];
            _rowCount = rowCount;
            _colCount = colCount;
        }

        public int[] GenerateValues()
        {
            const int maxRetryCount = 300;
            var retryCount = 0;
            var cells = new int[_rowCount * _colCount];
            while (retryCount < maxRetryCount)
            {
                retryCount++;
                PopulateCells(cells);
                ComputePrefixSum(cells);
                if (IsValidGrid()) return cells;
            }

            for (var i = 0; i < cells.Length; i++)
            {
                cells[i] = GameRule.DEFAULT_VALUE_WHEN_FAILED;
            }

            return cells;
        }

        #region Randomize

        private void PopulateCells(IList<int> cells)
        {
            SetWeightedRandomValue(cells);
            AdjustTotalSum(cells);
        }


        private void SetWeightedRandomValue(IList<int> cells)
        {
            for (var idx = 0; idx < cells.Count; idx++)
            {
                var randomValue = _numberRandomPicker.Pick();
                cells[idx] = randomValue;
            }
        }


        private void AdjustTotalSum(IList<int> cells)
        {
            var totalSum = cells.Sum();

            var remaining = totalSum % GameRule.TARGET_SUM;
            if (remaining == 0) return;

            var adjustment = GameRule.TARGET_SUM - remaining;
            var adjustedPairSum = GetAdjustedPairSum(cells[0], cells[^1], adjustment);
            var (newValue1, newValue2) = GetValidCellPair(adjustedPairSum);
            cells[0] = newValue1;
            cells[^1] = newValue2;
        }

        private int GetAdjustedPairSum(int value1, int value2, int adjustment)
        {
            var pairSum = value1 + value2 + adjustment;
            while (pairSum < GameRule.MIN_VALUE * 2)
            {
                pairSum += GameRule.TARGET_SUM;
            }

            while (pairSum > GameRule.MAX_VALUE * 2)
            {
                pairSum -= GameRule.TARGET_SUM;
            }

            return pairSum;
        }

        /// <summary>
        /// 합계를 만족하며 각 값이 [minValue, maxValue] 범위에 있는 두 cell 값 반환
        /// </summary>
        private (int, int) GetValidCellPair(int pairSum)
        {
            var lowerBound = Mathf.Max(GameRule.MIN_VALUE, pairSum - GameRule.MAX_VALUE);
            var upperBound = Mathf.Min(GameRule.MAX_VALUE, pairSum - GameRule.MIN_VALUE);
            var value1 = Random.Range(lowerBound, upperBound + 1);
            var value2 = pairSum - value1;
            return (value1, value2);
        }

        #endregion

        #region Prefix Sum

        private void ComputePrefixSum(int[] values)
        {
            for (var row = 0; row < _rowCount; row++)
            {
                for (var col = 0; col < _colCount; col++)
                {
                    _prefixSums[row + 1, col + 1] =
                        _prefixSums[row, col + 1] + _prefixSums[row + 1, col] - _prefixSums[row, col] +
                        values[row * _colCount + col];
                }
            }
        }

        private int GetRegionSum(Region region)
        {
            return _prefixSums[region.maxRow + 1, region.maxCol + 1] -
                   _prefixSums[region.minRow, region.maxCol + 1] -
                   _prefixSums[region.maxRow + 1, region.minCol] +
                   _prefixSums[region.minRow, region.minCol];
        }

        #endregion

        #region Valid Check

        private bool IsValidGrid()
        {
            var validRegionCount = 0;

            for (var minRow = 0; minRow < _rowCount; minRow++)
            {
                for (var minCol = 0; minCol < _colCount; minCol++)
                {
                    for (var maxRow = minRow; maxRow < _rowCount; maxRow++)
                    {
                        for (var maxCol = minCol; maxCol < _colCount; maxCol++)
                        {
                            var region = new Region(minRow, minCol, maxRow, maxCol);
                            if (IsMinimumValidRegion(region) is false) continue;

                            validRegionCount++;
                            if (validRegionCount > GameRule.MINIMUM_VALID_REGION_COUNT) return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool IsMinimumValidRegion(Region region)
        {
            if (IsValidRegion(region) is false) return false;

            if (region.minRow != region.maxRow &&
                IsValidRegion(region.ShrinkFromBottom) || IsValidRegion(region.ShrinkFromTop))
            {
                return false;
            }

            if (region.minCol != region.maxCol &&
                IsValidRegion(region.ShrinkFromLeft) || IsValidRegion(region.ShrinkFromRight))
            {
                return false;
            }

            return true;
        }


        private bool IsValidRegion(Region region)
        {
            return GetRegionSum(region) == GameRule.TARGET_SUM;
        }

        #endregion
    }
}