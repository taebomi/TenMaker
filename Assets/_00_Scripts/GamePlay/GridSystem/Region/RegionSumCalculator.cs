using System;
using TenMaker.Gameplay.GridSystem;
using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.GridSystem.Multiplay;

namespace TenMaker.Gameplay
{
    public class RegionSumCalculator
    {
        private readonly int[,] _prefixSumGrid;
        private readonly int _rowCount;
        private readonly int _colCount;

        private Grid _grid;

        public RegionSumCalculator(Grid grid)
        {
            _grid = grid;
            _rowCount = grid.RowCount;
            _colCount = grid.ColCount;
            _prefixSumGrid = new int[_rowCount + 1, _colCount + 1]; // 1-based index
        }

        public void ComputePrefixSum()
        {
            for (var row = 0; row < _rowCount; row++)
            {
                for (var col = 0; col < _colCount; col++)
                {
                    _prefixSumGrid[row + 1, col + 1] =
                        _grid[row, col].Value
                        + _prefixSumGrid[row, col + 1]
                        + _prefixSumGrid[row + 1, col]
                        - _prefixSumGrid[row, col];
                }
            }
        }

        public int GetRegionSum(Region region)
        {
            return _prefixSumGrid[region.maxRow + 1, region.maxCol + 1]
                   - _prefixSumGrid[region.minRow, region.maxCol + 1]
                   - _prefixSumGrid[region.maxRow + 1, region.minCol]
                   + _prefixSumGrid[region.minRow, region.minCol];
        }

        public int GetRegionSum(int minRow, int minCol, int maxRow, int maxCol)
        {
            return _prefixSumGrid[maxRow + 1, maxCol + 1]
                   - _prefixSumGrid[minRow, maxRow + 1]
                   - _prefixSumGrid[maxRow + 1, minCol]
                   + _prefixSumGrid[minRow, minCol];
        }
    }
}