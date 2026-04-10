using UnityEngine;
using UnityEngine.Serialization;

namespace TenMaker.Core
{
    public class Cell : MonoBehaviour
    {
        public const float SIZE = 1f;
        public const float HALF_SIZE = SIZE / 2f;
        
        public int Value { get; private set; }
        public Vector2Int Coordinate { get; private set; }

        [SerializeField] private SpriteRenderer cellSr;
        [SerializeField] private SpriteRenderer valueSr;
        [SerializeField] private Sprite[] valueSprites;

        public void Setup(Vector2 gridCenterOffset, int col, int row)
        {
            Coordinate = new Vector2Int(col, row);
            transform.localPosition = new Vector3(col - gridCenterOffset.x, row - gridCenterOffset.y);
        }

        public void Initialize(int col, int row, Vector3 offset)
        {
            Coordinate = new Vector2Int(col, row);
            transform.localPosition = offset;
        }

        public void SetVisibility(bool value)
        {
            valueSr.enabled = value;
        }

        public void UpdateValue(int newValue)
        {
            Value = newValue;
            valueSr.sprite = valueSprites[Value];
            cellSr.enabled = true;
        }

        public void Clear()
        {
            Value = 0;
            valueSr.sprite = null;
            cellSr.enabled = false;
        }

        public void SetCellSprite(Sprite sprite)
        {
            cellSr.sprite = sprite;
        }
    }
}