using UnityEngine;

namespace TenMaker.Gameplay
{
    public interface ICustomCellBackgroundProvider
    {
        CellBackground GetTile(Vector2Int coordinate, Vector3 position, Transform parent);
    }
}