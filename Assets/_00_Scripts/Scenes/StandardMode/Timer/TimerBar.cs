using System;
using UnityEngine;
using UnityEngine.UI;

namespace TenMaker.RankMode
{
    [RequireComponent(typeof(RectTransform))]
    public class TimerBar : MonoBehaviour
    {
        [SerializeField] private RectTransform rt;
        [SerializeField] private Image image;
        [SerializeField] private float width;

        [SerializeField] private Gradient gradient;

        [SerializeField] private float shakeAmount = 3f;
        [SerializeField] private float shakeSpeed = 200f;

        private float _yPos;
        private float _totalTime;

        private void Awake()
        {
            _yPos = rt.anchoredPosition.y;
        }

        public void Setup(float totalTime)
        {
            _totalTime = totalTime;
            image.color = gradient.Evaluate(1f);
        }

        public void UpdateTimer(float remainedTime)
        {
            var progress = remainedTime / _totalTime;
            rt.sizeDelta = new Vector2(width * progress, rt.sizeDelta.y);
            image.color = gradient.Evaluate(progress);
            if (remainedTime < 5f)
            {
                ShakeBar();
            }
        }

        private void ShakeBar()
        {
            var y = _yPos + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            rt.anchoredPosition = new Vector2(0f, y);
        }

#if UNITY_EDITOR


        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += Update;

            void Update()
            {
                UnityEditor.EditorApplication.delayCall -= Update;
                if (rt == null || image == null) return;
                rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
                image.color = gradient.Evaluate(1f);
            }
        }
#endif
    }
}