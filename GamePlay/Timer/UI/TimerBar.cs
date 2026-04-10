using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace TenMaker.Gameplay.Timer
{
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class TimerBar : MonoBehaviour
    {
        [SerializeField, ReadOnly] private float width;
        [SerializeField, ReadOnly] private float height;
        [SerializeField, ReadOnly] private float yPosition;
        [SerializeField] private Gradient gradient;
        [SerializeField] private float shakeSpeed;
        [SerializeField] private float shakeAmount;

        private RectTransform _rt;
        private Image _image;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        public void ResetVisual()
        {
            _rt.anchoredPosition = new Vector2(0f, yPosition);
        }

        public void UpdateRatio(float ratio)
        {
            _rt.sizeDelta = new Vector2(width * ratio, _rt.sizeDelta.y);
            _image.color = gradient.Evaluate(ratio);
        }

        /// <summary>
        /// 연속적으로 호출 필요
        /// </summary>
        public void ShakeYPosition()
        {
            var randomY = yPosition + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, randomY);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_rt == null) _rt = GetComponent<RectTransform>();

            if (_image == null) _image = GetComponent<Image>();

            width = _rt.sizeDelta.x;
            yPosition = _rt.anchoredPosition.y;
            _image.color = gradient.Evaluate(1f);
        }
#endif
    }
}