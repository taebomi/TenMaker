using TenMaker.Gameplay.Customization;
using UnityEngine;
using UnityEngine.Pool;

namespace TenMaker.Gameplay
{
    public class CheckeredCellBackgroundProvider : MonoBehaviour, ICustomCellBackgroundProvider
    {
        [SerializeField] private CheckeredBackground backgroundPrefab;

        public CellBackground GetTile(Vector2Int coordinate, Vector3 position, Transform parent)
        {
            var tile = Instantiate(backgroundPrefab, position, Quaternion.identity, parent);
            var isBase = (coordinate.x + coordinate.y) % 2 == 0;
            tile.SetType(isBase);
            tile.SetDefaultColor();
            return tile;
        }
    }
}