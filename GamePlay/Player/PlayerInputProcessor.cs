using System;
using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Gameplay.Player
{
    public class PlayerInputProcessor
    {
        public bool IsDragging { get; private set; }
        public GridSelectionData CurrentDragData { get; private set; }

        private readonly Camera _cam;
        private readonly Func<Vector2, CellObject> _getCell;
        private readonly Func<Vector2, CellObject> _getNearestCell;

        public event Action<GridSelectionData> DragPerformed;
        public event Action<GridSelectionData> DragCanceled;

        public PlayerInputProcessor(Camera cam, Func<Vector2, CellObject> getCell, Func<Vector2, CellObject> getNearestCell)
        {
            _cam = cam;
            _getCell = getCell;
            _getNearestCell = getNearestCell;
        }

        public void OnDragStarted(Vector2 point)
        {
            var worldPos = _cam.ScreenToWorldPoint(point);
            var cell = _getCell(worldPos);
            if (cell == null) return;

            IsDragging = true;
            CurrentDragData = new GridSelectionData(cell.Coordinate);
            DragPerformed?.Invoke(CurrentDragData);
        }

        public void OnDragPerformed(Vector2 point)
        {
            if (IsDragging is false) return;

            var worldPos = _cam.ScreenToWorldPoint(point);
            var selectedCell = _getNearestCell(worldPos);
            var newDragData = new GridSelectionData(CurrentDragData.StartCoordinate, selectedCell.Coordinate);

            if (CurrentDragData == newDragData) return;
            CurrentDragData = newDragData;
            DragPerformed?.Invoke(CurrentDragData);
        }

        public void OnDragCanceled()
        {
            if (IsDragging is false) return;

            IsDragging = false;
            DragCanceled?.Invoke(CurrentDragData);
        }
    }
}