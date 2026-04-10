using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TenMaker.Core;
using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.Player.Multiplay;
using TenMaker.Utility;
using UnityEngine;
using Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;
using GridSystem_Multiplay_Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;
using Multiplay_Grid = TenMaker.Gameplay.GridSystem.Multiplay.Grid;

namespace TenMaker.Gameplay.Multiplay
{
    public class GridVisualizer : MonoBehaviour
    {
        public CellObject[,] CellObjects { get; private set; }
        public Vector2 CenterPosition { get; private set; } // Grid 중앙 위치
        public Vector2 CenterOffset { get; private set; }
        public Vector2 OriginPosition { get; private set; } // Grid 왼쪽 하단 위치

        private GridSystem_Multiplay_Grid Grid { get; set; }

        [SerializeField] private CellObject cellObjectPrefab;

        private PlayersContext _playerContext;
        private FlipAnimator _flipAnimator;

        //
        // INITIALIZE METHODS
        //
        public void InitializeClient(GridSystem_Multiplay_Grid grid, PlayersContext playerContext)
        {
            Grid = grid;
            _playerContext = playerContext;
            _flipAnimator = new FlipAnimator(this);
            CenterPosition = transform.position;
            CenterOffset = new Vector2(grid.ColCount * CellObject.HALF_SIZE - CellObject.HALF_SIZE,
                grid.RowCount * CellObject.HALF_SIZE - CellObject.HALF_SIZE);
            OriginPosition = CenterPosition - CenterOffset;

            InitializeCellObjects(playerContext);
        }

        private void InitializeCellObjects(PlayersContext playerContext)
        {
            CellObjects = new CellObject[Grid.RowCount, Grid.ColCount];

            var localPlayerCustomizationSystem = playerContext.LocalPlayer.CustomizationSystem;
            var cellBackgroundProvider = localPlayerCustomizationSystem.GetCellBackgroundProvider();
            var cellNumberPrefab = localPlayerCustomizationSystem.GetCellNumber();

            for (var row = 0; row < Grid.RowCount; row++)
            {
                for (var col = 0; col < Grid.ColCount; col++)
                {
                    var position = new Vector2(col * CellObject.SIZE, row * CellObject.SIZE) + OriginPosition;
                    var cellObject = Instantiate(cellObjectPrefab, position, Quaternion.identity, transform);
                    var cellBackground =
                        cellBackgroundProvider.GetTile(new Vector2Int(col, row), position, cellObject.transform);
                    var cellNumber = Instantiate(cellNumberPrefab, position, Quaternion.identity, cellObject.transform);
                    cellObject.Initialize(Grid[row, col], cellBackground, cellNumber, new Vector2Int(col, row));
                    CellObjects[row, col] = cellObject;
                }
            }
        }

        //
        // PUBLIC METHODS
        //

        public Vector2 GetRegionCenter(Region region)
        {
            var centerX = (region.minCol + region.maxCol) * 0.5f * CellObject.SIZE;
            var centerY = (region.minRow + region.maxRow) * 0.5f * CellObject.SIZE;
            return new Vector2(centerX, centerY) + OriginPosition;
        }

        public void ApplyClearedRegion(ClearEventContext clearEvent)
        {
            foreach (var cellObject in clearEvent.ClearedCells)
            {
                cellObject.SetValueVisible(false);
            }

            var playerColor = _playerContext.AllPlayers[clearEvent.ClientId].Color;
            _flipAnimator.FlipAsync(playerColor, clearEvent.Region, destroyCancellationToken).Forget();
        }

        /// <summary>
        /// 해당 영역을 플레이어의 영역으로 시각화
        /// cell objects 반환
        /// </summary>
        public List<CellObject> ClearCellObjects(ClearEventDTO clearEventDTO, List<Vector2Int> clearedCoordinates)
        {
            var clearedCellObjects = new List<CellObject>();
            foreach (var coordinate in clearedCoordinates)
            {
                var cellObject = CellObjects[coordinate.y, coordinate.x];
                cellObject.SetValueVisible(false);
                clearedCellObjects.Add(cellObject);
            }

            var playerColor = _playerContext.AllPlayers[clearEventDTO.ClientId].Color;
            var region = clearEventDTO.ClearedRegion;

            _flipAnimator.FlipAsync(playerColor, region, destroyCancellationToken).Forget();
            return clearedCellObjects;
        }


        /// <summary>
        /// 셀의 값 표시 설정
        /// </summary>
        /// <param name="value"></param>
        public void SetVisibility(bool value)
        {
            foreach (var cell in CellObjects)
            {
                cell.SetValueVisible(value);
            }
        }

        /// <summary>
        /// <paramref name="worldPosition"/>에 대응하는 CellObject를 반환
        /// 없을 경우 null 반환 
        /// </summary>
        public CellObject GetCellObject(Vector2 worldPosition)
        {
            var gridPoint = worldPosition - OriginPosition;
            var col = Mathf.RoundToInt(gridPoint.x);
            var row = Mathf.RoundToInt(gridPoint.y);
            if (row < 0 || row >= Grid.RowCount || col < 0 || col >= Grid.ColCount) return null;
            return CellObjects[row, col];
        }

        /// <summary>
        /// 해당 worldPosition에 가장 가까운 CellObject를 반환
        /// </summary>
        public CellObject GetNearestCell(Vector2 worldPosition)
        {
            var gridPoint = worldPosition - OriginPosition;
            var col = Mathf.Clamp(Mathf.RoundToInt(gridPoint.x), 0, Grid.ColCount - 1);
            var row = Mathf.Clamp(Mathf.RoundToInt(gridPoint.y), 0, Grid.RowCount - 1);
            return CellObjects[row, col];
        }

        /// <summary>
        /// 주어진 좌표 배열에 해당하는 CellObject들을 반환 
        /// </summary>
        public List<CellObject> GetCellObjects(Vector2Int[] coordinates)
        {
            var cellObjects = new List<CellObject>(coordinates.Length);
            foreach (var coordinate in coordinates)
            {
                if (coordinate.y < 0 || coordinate.y >= Grid.RowCount || coordinate.x < 0 ||
                    coordinate.x >= Grid.ColCount)
                {
                    TBMLog.HeaderError($"Invalid coordinate : {coordinate}");
                    continue;
                }

                var cellObject = CellObjects[coordinate.y, coordinate.x];
                cellObjects.Add(cellObject);
            }

            return cellObjects;
        }

        /// <summary>
        /// 두 셀을 받아 Region으로 반환
        /// </summary>
        public Region GetRegion(CellObject cell1, CellObject cell2)
        {
            int minRow, minCol, maxRow, maxCol;
            if (cell1.Coordinate.x <= cell2.Coordinate.x)
            {
                minCol = cell1.Coordinate.x;
                maxCol = cell2.Coordinate.x;
            }
            else
            {
                minCol = cell2.Coordinate.x;
                maxCol = cell1.Coordinate.x;
            }

            if (cell1.Coordinate.y <= cell2.Coordinate.y)
            {
                minRow = cell1.Coordinate.y;
                maxRow = cell2.Coordinate.y;
            }
            else
            {
                minRow = cell2.Coordinate.y;
                maxRow = cell1.Coordinate.y;
            }

            return new Region(minRow, minCol, maxRow, maxCol);
        }

        /// <summary>
        /// Region을 사각형의 중심과 크기로 반환
        /// </summary>
        /// <param name="selectedRegion"></param>
        /// <returns></returns>
        public (Vector3 centerPos, Vector2 size) ConvertToArea(Region selectedRegion)
        {
            var width = selectedRegion.maxCol - selectedRegion.minCol + CellObject.SIZE;
            var height = selectedRegion.maxRow - selectedRegion.minRow + CellObject.SIZE;
            var centerX = (selectedRegion.minCol + selectedRegion.maxCol) * 0.5f;
            var centerY = (selectedRegion.minRow + selectedRegion.maxRow) * 0.5f;

            var centerPos = new Vector3(centerX + OriginPosition.x, centerY + OriginPosition.y);
            var size = new Vector2(width, height);
            return (centerPos, size);
        }
    }
}