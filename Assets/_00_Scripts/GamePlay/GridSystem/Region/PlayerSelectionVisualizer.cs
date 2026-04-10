using UnityEngine;

namespace TenMaker.Gameplay
{
    public class PlayerSelectionVisualizer : MonoBehaviour
    {
        private const float OWNER_ALPHA = 1f;
        private const float OTHER_ALPHA = 0.5f;

        [SerializeField] private GridCursor cursor;
        [SerializeField] private SpriteRenderer areaSr;

        private float _alpha;

        public void Initialize(bool isOwner)
        {
            SetOwner(isOwner);
            Hide();
        }

        public void UpdateVisual(Vector3 position, Vector2 size)
        {
            if (gameObject.activeSelf is false)
            {
                gameObject.SetActive(true);
            }

            transform.position = position;
            cursor.SetSize(size);
            areaSr.size = size;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetGameObjectName(string newName)
        {
            gameObject.name = newName;
        }

        private void SetOwner(bool value)
        {
            _alpha = value ? OWNER_ALPHA : OTHER_ALPHA;
            cursor.SetAlpha(_alpha);
            var color = areaSr.color;
            color = new Color(color.r, color.g, color.b, _alpha);
            cursor.SetAlpha(_alpha);
            areaSr.color = color;
        }
    }
}