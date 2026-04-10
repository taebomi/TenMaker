using TenMaker.Core;
using TenMaker.Utility;
using TenMaker.Gameplay.GridSystem;
using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.GridSystem.Multiplay;

namespace TenMaker.Gameplay
{
    public class ValidRegionChecker
    {
        private readonly int[,] _prefixSumGrid;
        private readonly Grid _grid;

        public ValidRegionChecker(Grid grid)
        {
            _grid = grid;
            _prefixSumGrid = new int[grid.RowCount + 1, grid.ColCount + 1];
            Compute();
        }

        /// <summary>
        /// Prefix Sum 계산, Grid 변경 시마다 호출 필요
        /// </summary>
        public void Compute()
        {
            for (var row = 0; row < _grid.RowCount; row++)
            {
                for (var col = 0; col < _grid.ColCount; col++)
                {
                    var cell = _grid[row, col];
                    var value = cell.Cleared ? 0 : cell.Value;
                    _prefixSumGrid[row + 1, col + 1] =
                        value + _prefixSumGrid[row, col + 1] + _prefixSumGrid[row + 1, col] - _prefixSumGrid[row, col];
                }
            }
        }

        /// <summary>
        /// 주어진 영역의 합을 계산 
        /// </summary>
        public int GetRegionSum(Region region)
        {
            return _prefixSumGrid[region.maxRow + 1, region.maxCol + 1] -
                   _prefixSumGrid[region.minRow, region.maxCol + 1] -
                   _prefixSumGrid[region.maxRow + 1, region.minCol] +
                   _prefixSumGrid[region.minRow, region.minCol];
        }

        /// <summary>
        /// 주어진 영역이 유효한지 확인 
        /// </summary>
        public bool IsValidRegion(Region region)
        {
            return GetRegionSum(region) == GameRule.TARGET_SUM;
        }

        /// <summary>
        /// 최소 유효 영역으로 줄이기
        /// </summary>
        public Region TrimMinimalValidRegion(Region region)
        {
            while (region.CanShrinkRow && IsValidRegion(region.ShrinkFromBottom))
            {
                region = region.ShrinkFromBottom;
            }

            while (region.CanShrinkRow && IsValidRegion(region.ShrinkFromTop))
            {
                region = region.ShrinkFromTop;
            }

            while (region.CanShrinkCol && IsValidRegion(region.ShrinkFromLeft))
            {
                region = region.ShrinkFromLeft;
            }

            while (region.CanShrinkCol && IsValidRegion(region.ShrinkFromRight))
            {
                region = region.ShrinkFromRight;
            }

            return region;
        }
    }
}