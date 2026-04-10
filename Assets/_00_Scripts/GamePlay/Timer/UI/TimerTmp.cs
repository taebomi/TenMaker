using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Utility;
using TMPro;
using UnityEngine;

namespace TenMaker.Gameplay.Timer
{
    [RequireComponent(typeof(RectTransform), typeof(TMP_Text))]
    public class TimerTmp : MonoBehaviour
    {
        private const int LOW_SEC = 5;
        
        [SerializeField] private float shakeAmount;
        [SerializeField] private float shakeSpeed;

        private RectTransform _rt;
        private TMP_Text _tmp;

        private int _lastSec;
        private Vector2 _oriPosition;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _tmp = GetComponent<TMP_Text>();

            _oriPosition = _rt.anchoredPosition;
        }

        public void Setup(float time)
        {
            var sec = Mathf.CeilToInt(time);
            _tmp.text = FormatTime(sec);
            ResetVisual();
        }

        public void UpdateTimer(float time)
        {
            var sec = Mathf.CeilToInt(time);
            if (sec == _lastSec) return;
            
            _lastSec = sec;
            _tmp.text = FormatTime(sec);
            PulseAsync(destroyCancellationToken).Forget();
        }
        
        public void SetColor(Color color)
        {
            _tmp.color = color;
        }

        public void ResetVisual()
        {
            _rt.localScale = Vector3.one;
            _rt.anchoredPosition = _oriPosition;
            _tmp.color = Color.white;
        }

        /// <summary>
        /// 연속적으로 호출 필요
        /// </summary>
        public void ShakePosition()
        {
            var xPosition = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            var yPosition = Mathf.Cos(Time.time * shakeSpeed * 0.85f) * shakeAmount;
            _rt.anchoredPosition = _oriPosition + new Vector2(xPosition, yPosition);
        }

        private async UniTaskVoid PulseAsync(CancellationToken ct)
        {
            const float inDuration = 0.1f;
            const float outDuration = 0.2f;
            const float scale = 1.25f;
            const float oriScale = 1f;

            var timer = 0f;
            var scaleDiff = scale - oriScale;
            while (timer < inDuration && ct.IsCancellationRequested is false)
            {
                var curScale = oriScale + scaleDiff * TBMUtility.OutBack(timer, inDuration);
                _rt.localScale = new Vector3(curScale, curScale, 1f);
                timer += Time.deltaTime;
                await UniTask.Yield(ct);
            }

            timer = 0f;
            while (timer < outDuration && ct.IsCancellationRequested is false)
            {
                var curScale = oriScale + scaleDiff * (1 - TBMUtility.InOutSine(timer, outDuration));
                _rt.localScale = new Vector3(curScale, curScale, 1f);
                timer += Time.deltaTime;
                await UniTask.Yield(ct);
            }
        }

        private string FormatTime(int sec)
        {
            if (sec >= 60)
            {
                var minutes = sec / 60;
                var seconds = sec % 60;
                return $"{minutes:0}:{seconds:00}";
            }
            else
            {
                var seconds = sec;
                if (seconds < 0) seconds = 0;
                return $"{seconds}";
            }
        }
    }
}