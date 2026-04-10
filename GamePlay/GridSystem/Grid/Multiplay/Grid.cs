using System.Collections.Generic;
using TenMaker.Core;
using TenMaker.Utility;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Gameplay.GridSystem.Multiplay
{
    public class Grid
    {
        public Cell[,] Cells { get; private set; }
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
        public int NotClearedCellCount { get; private set; }

        public Cell this[int row, int col] => Cells[row, col];
        public Cell this[int index]
        {
            get => Cells[index / ColCount, index % ColCount];
            set => Cells[index / ColCount, index % ColCount] = value;
        }

        public Grid(GridConfigData configData, int[] values)
        {
            RowCount = configData.rowCount;
            ColCount = configData.colCount;
            NotClearedCellCount = RowCount * ColCount;
            Cells = new Cell[RowCount, ColCount];
            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColCount; col++)
                {
                    var index = row * ColCount + col;
                    Cells[row, col] = new Cell(values[index]);
                }
            }
        }

        public Grid(GridInitializationDTO initializationData)
        {
            var configData = initializationData.ConfigData;
            var cellData = initializationData.CellData;
            RowCount = configData.rowCount;
            ColCount = configData.colCount;
            NotClearedCellCount = RowCount * ColCount;
            Cells = new Cell[RowCount, ColCount];
            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColCount; col++)
                {
                    Cells[row, col] = new Cell(cellData[row * ColCount + col]);
                }
            }
        }


        /// <summary>
        /// 모든 셀 값 설정
        /// </summary>
        /// <param name="cells"></param>
        public void SetCellValues(IEnumerable<NetworkCellData> cells)
        {
            using var enumerator = cells.GetEnumerator();
            NotClearedCellCount = RowCount * ColCount;
            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColCount; col++)
                {
                    if (enumerator.MoveNext() is false)
                    {
                        TBMLog.HeaderError("cell 개수 부족");
                        return;
                    }

                    var cell = Cells[row, col];
                    var cellData = enumerator.Current;
                    cell.Apply(cellData);
                }
            }

            if (enumerator.MoveNext())
            {
                TBMLog.HeaderError("cell 개수 초과");
                return;
            }
        }

        /// <summary>
        /// region 영억 모두 해당 clientId로 Clear
        /// clearedCoordinates에 clear된 cell의 좌표를 추가
        /// </summary>
        /// <returns></returns>
        public List<Vector2Int> ClearRegion(ulong clientId, Region region)
        {
            var coordinate = new List<Vector2Int>(region.Area);
            for (var row = region.minRow; row <= region.maxRow; row++)
            {
                for (var col = region.minCol; col <= region.maxCol; col++)
                {
                    var cell = Cells[row, col];
                    if (cell.Cleared is false)
                    {
                        coordinate.Add(new Vector2Int(col, row));
                    }

                    cell.Clear(clientId);
                }
            }

            return coordinate;
        }

        public void ApplyClearedRegion(ulong clientId, Region region)
        {
            for (var row = region.minRow; row <= region.maxRow; row++)
            {
                for (var col = region.minCol; col <= region.maxCol; col++)
                {
                    Cells[row, col].Clear(clientId);
                }
            }
        }
    }
}