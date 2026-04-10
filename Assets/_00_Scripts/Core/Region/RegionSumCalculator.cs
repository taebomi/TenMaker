using System;
using UnityEngine;

namespace TenMaker.Core
{
    public class RegionSumCalculator : MonoBehaviour
    {
        private GridSystem _gridSystem;

        private int[,] _prefixSumGrid;
        private int _rowCount;
        private int _colCount;

        public void Setup(GridSystem gridSystem)
        {
            _gridSystem = gridSystem;
            _rowCount = gridSystem.RowCount;
            _colCount = gridSystem.ColCount;
            _prefixSumGrid = new int[_rowCount + 1, _colCount + 1]; // 1-based index
        }

        /// <summary>
        /// 누적합 계산하여 저장
        /// </summary>
        public void ComputePrefixSum()
        {
            for (var row = 1; row <= _rowCount; row++)
            {
                for (var col = 1; col <= _colCount; col++)
                {
                    _prefixSumGrid[row, col] =
                        _gridSystem[row - 1, col - 1].Value // 0-based index
                        + _prefixSumGrid[row - 1, col]
                        + _prefixSumGrid[row, col - 1]
                        - _prefixSumGrid[row - 1, col - 1];
                }
            }
        }

        /// <summary>
        /// 누적합을 이용해 영역의 합 계산
        /// 반드시 start가 end보다 작을 것
        /// </summary>
        public int GetRegionSum(int minRow, int minCol, int maxRow, int maxCol)
        {
            return _prefixSumGrid[maxRow + 1, maxCol + 1]
                   - _prefixSumGrid[minRow, maxCol + 1]
                   - _prefixSumGrid[maxRow + 1, minCol]
                   + _prefixSumGrid[minRow, minCol];
        }

        public int GetRegionSum(Cell startCell, Cell endCell)
        {
            var (minRow, minCol, maxRow, maxCol) = _gridSystem.GetRegionBounds(startCell, endCell);
            return GetRegionSum(minRow, minCol, maxRow, maxCol);
        }
    }
}