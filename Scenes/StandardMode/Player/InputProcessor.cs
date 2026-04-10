using System;
using TenMaker.Core;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class InputProcessor : MonoBehaviour
    {
        public Cell StartCell { get; private set; }
        public Cell EndCell { get; private set; }

        private Func<Vector2, Cell> _getCell;
        private Func<Vector2, Cell> _getNearestCell;

        public event Action DragStarted;
        public event Action<Cell, Cell> DragPerformed;
        public event Action<Cell, Cell> DragCanceled;

        public void Setup(Func<Vector2, Cell> getCellFunc, Func<Vector2, Cell> getNearestCellFunc)
        {
            _getCell = getCellFunc;
            _getNearestCell = getNearestCellFunc;
        }

        public void OnDragStarted(Vector2 pos)
        {
            EndCell = StartCell = _getCell.Invoke(pos);

            if (StartCell is null) return;
            DragStarted?.Invoke();
        }

        public void OnDragPerformed(Vector2 pos)
        {
            if (StartCell is null) return;

            EndCell = _getNearestCell.Invoke(pos);
            DragPerformed?.Invoke(StartCell, EndCell);
        }

        public void OnDragCanceled()
        {
            if (StartCell is null) return;

            DragCanceled?.Invoke(StartCell, EndCell);
            StartCell = null;
            EndCell = null;
        }
    }
}