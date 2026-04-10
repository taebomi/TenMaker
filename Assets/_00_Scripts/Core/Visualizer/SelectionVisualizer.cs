using System;
using UnityEngine;

namespace TenMaker.Core
{
    public class SelectionVisualizer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer areaSr;
        [SerializeField] private SpriteRenderer cursorSr;

        public void Show()
        {
            areaSr.gameObject.SetActive(true);
            cursorSr.gameObject.SetActive(true);
        }

        public void Hide()
        {
            areaSr.gameObject.SetActive(false);
            cursorSr.gameObject.SetActive(false);
        }

        public void UpdateSelection(Cell startCell, Cell endCell)
        {
            // get centerPos
            var startPos = startCell.transform.position;
            var endPos = endCell.transform.position;
            var centerPos = (startPos + endPos) * 0.5f;

            // get size
            var minX = Mathf.Min(startPos.x, endPos.x) - 0.5f;
            var minY = Mathf.Min(startPos.y, endPos.y) - 0.5f;
            var maxX = Mathf.Max(startPos.x, endPos.x) + 0.5f;
            var maxY = Mathf.Max(startPos.y, endPos.y) + 0.5f;
            var size = new Vector2(maxX - minX, maxY - minY);

            // apply
            areaSr.transform.position = centerPos;
            cursorSr.transform.position = centerPos;
            areaSr.size = size;
            cursorSr.size = size;
        }
    }
}