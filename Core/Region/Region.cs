using UnityEngine;

namespace TenMaker.Core
{
    public struct Region
    {
        public Vector2 Center;
        public Vector2 Size;

        public Region(Cell startCell, Cell endCell)
        {
            Center = (startCell.transform.position + endCell.transform.position) * 0.5f;
            var width = endCell.Coordinate.x - startCell.Coordinate.x + 1;
            var height = endCell.Coordinate.y - startCell.Coordinate.y + 1;
            Size = new Vector2(width, height);
        }
    }
}