using System;
using System.Collections.Generic;
using TenMaker.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TenMaker.Core
{
    public class GridSystem : MonoBehaviour
    {
        public Cell[,] Cells { get; private set; }

        public int RowCount { get; private set; }
        public int ColCount { get; private set; }

        public Vector2 CenterPosition { get; private set; } // Grid 중앙 위치
        public Vector2 CenterOffset { get; private set; }
        public Vector2 OriginPosition { get; private set; } // Grid 왼쪽 하단 위치

        public int ValueCellCount { get; private set; }

        public Cell this[int row, int col] => Cells[row, col];

        [SerializeField] private Sprite sr1, sr2;
        [SerializeField] private Cell cellPrefab;

        #region Initialize Methods

        public void Setup(int col, int row)
        {
            Cells = new Cell[row, col];
            RowCount = row;
            ColCount = col;

            CenterPosition = transform.position;
            CenterOffset = new Vector2(ColCount * 0.5f - 0.5f, RowCount * 0.5f - 0.5f);
            OriginPosition = CenterPosition - CenterOffset;
            CreateCellObjects();
        }

        #endregion

        public void CreateGrid()
        {
            RandomizeGrid();
            AdjustTotalSum();
            ValueCellCount = RowCount * ColCount;
        }

        public void SetVisibility(bool value)
        {
            foreach (var cell in Cells)
            {
                cell.SetVisibility(value);
            }
        }

        /// <summary>
        /// 입력한 위치에 대응하는 Cell 반환
        /// 만약 해당 위치에 Cell이 존재하지 않는다면 null 반환
        /// </summary>
        public Cell GetCell(Vector2 worldPosition)
        {
            var gridPoint = worldPosition - OriginPosition;
            var col = Mathf.RoundToInt(gridPoint.x);
            var row = Mathf.RoundToInt(gridPoint.y);

            if (row < 0 || row >= RowCount || col < 0 || col >= ColCount) return null;

            return Cells[row, col];
        }

        /// <summary>
        /// 입력한 위치가 그리드 바깥일 경우 해당 위치로부터 가장 가까운 Cell 반환
        /// </summary>
        public Cell GetNearestCell(Vector2 worldPosition)
        {
            var gridPoint = worldPosition - OriginPosition;
            var col = Mathf.Clamp(Mathf.RoundToInt(gridPoint.x), 0, ColCount - 1);
            var row = Mathf.Clamp(Mathf.RoundToInt(gridPoint.y), 0, RowCount - 1);

            return Cells[row, col];
        }

        /// <summary>
        /// 순서상관없는 Cell 두개를 입력받아 직사각형 영역의 최소/최대 좌표 반환
        /// </summary>
        /// <returns>minRow, minCol, maxRow, maxCol</returns>
        public (int, int, int, int) GetRegionBounds(Cell startCell, Cell endCell)
        {
            int minRow, minCol, maxRow, maxCol;

            var startCoordinate = startCell.Coordinate;
            var endCoordinate = endCell.Coordinate;

            if (startCoordinate.x <= endCoordinate.x)
            {
                minCol = startCoordinate.x;
                maxCol = endCoordinate.x;
            }
            else
            {
                minCol = endCoordinate.x;
                maxCol = startCoordinate.x;
            }

            if (startCoordinate.y <= endCoordinate.y)
            {
                minRow = startCoordinate.y;
                maxRow = endCoordinate.y;
            }
            else
            {
                minRow = endCoordinate.y;
                maxRow = startCoordinate.y;
            }

            return (minRow, minCol, maxRow, maxCol);
        }

        /// <summary>
        /// 순서무관한 두 Cell의 영역을 제거하고 0이 아닌 Cell 개수 반환
        /// </summary>
        public int ClearRegion(Cell startCell, Cell endCell, List<Cell> clearedCells)
        {
            clearedCells.Clear();

            var (minRow, minCol, maxRow, maxCol) = GetRegionBounds(startCell, endCell);

            var clearedCellCount = 0;
            for (var row = minRow; row <= maxRow; row++)
            {
                for (var col = minCol; col <= maxCol; col++)
                {
                    if (Cells[row, col].Value == 0) continue;

                    Cells[row, col].Clear();
                    clearedCells.Add(Cells[row, col]);
                    clearedCellCount++;
                }
            }

            ValueCellCount -= clearedCellCount;
            return clearedCellCount;
        }

        #region Create

        private void CreateCellObjects()
        {
            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColCount; col++)
                {
                    var cell = Instantiate(cellPrefab, transform);
                    cell.Setup(CenterOffset, col, row);
                    var isEvent = (row + col) % 2 == 0;
                    cell.SetCellSprite(isEvent ? sr1 : sr2);
                    Cells[row, col] = cell;
                }
            }
        }

        private void RandomizeGrid()
        {
            foreach (var cell in Cells)
            {
                cell.UpdateValue(WeightedRandomPicker.Pick());
            }
        }

        /// <summary>
        /// 총합을 10의 배수로 보정
        /// </summary>
        private void AdjustTotalSum()
        {
            // 총 합 계산
            var totalSum = 0;
            foreach (var cell in Cells)
            {
                totalSum += cell.Value;
            }

            // 이미 10의 배수일 경우 패스
            var remaining = totalSum % ValidRegionProcessor.REGION_SUM;
            if (remaining == 0) return;

            // 총합을 10의 배수로 보정
            var adjustment = ValidRegionProcessor.REGION_SUM - remaining;

            var cell1 = Cells[0, 0];
            var cell2 = Cells[RowCount - 1, ColCount - 1];

            var adjustedPairSum = GetAdjustedPairSum(cell1.Value, cell2.Value, adjustment);
            var (newValue1, newValue2) = GetValidCellPair(adjustedPairSum);

            cell1.UpdateValue(newValue1);
            cell2.UpdateValue(newValue2);
        }

        /// <summary>
        /// 보정값과 현재 값 두개로부터 새로운 값 반환
        /// </summary>
        private int GetAdjustedPairSum(int value1, int value2, int adjustment)
        {
            var pairSum = value1 + value2 + adjustment;

            while (pairSum < 2) // 1, 1 미만
            {
                pairSum += ValidRegionProcessor.REGION_SUM;
            }

            while (pairSum > 18) // 9, 9 초과
            {
                pairSum -= ValidRegionProcessor.REGION_SUM;
            }

            return pairSum;
        }

        /// <summary>
        /// pairSum 값이 되는 두 cell 값 반환
        /// </summary>
        private (int, int) GetValidCellPair(int pairSum)
        {
            var lowerBound = Mathf.Max(1, pairSum - 9);
            var upperBound = Mathf.Min(9, pairSum - 1);
            var newValue1 = Random.Range(lowerBound, upperBound + 1);
            var newValue2 = pairSum - newValue1;

            return (newValue1, newValue2);
        }

        #endregion

        /// <summary>
        /// 값이 존재하는 Cell의 위치를 섞음
        /// </summary>
        [ContextMenu("Shuffle Cells")]
        public void ShuffleCells()
        {
            // 0이 아닌 값을 수집
            var values = new List<int>(ValueCellCount);
            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColCount; col++)
                {
                    var cell = Cells[row, col];
                    if (cell.Value == 0) continue;

                    values.Add(cell.Value);
                }
            }

            // fisher-yates shuffle
            for (var i = 0; i < values.Count; i++)
            {
                var randomIndex = Random.Range(i, values.Count);
                (values[i], values[randomIndex]) = (values[randomIndex], values[i]);
            }

            // 0이 아닌 값을 셔플된 값으로 업데이트
            var index = 0;
            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColCount; col++)
                {
                    var cell = Cells[row, col];
                    if (cell.Value == 0) continue;

                    cell.UpdateValue(values[index]);
                    index++;
                }
            }
        }
    }
}