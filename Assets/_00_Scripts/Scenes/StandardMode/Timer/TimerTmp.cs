using Cysharp.Threading.Tasks;
using TenMaker.Utility;
using TMPro;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class TimerTmp : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmp;
        [SerializeField] private RectTransform rt;

        [SerializeField] private float shakeAmount = 2.5f;
        [SerializeField] private float shakeSpeed = 200f;

        private int _lastSec;

        public void Setup(float time)
        {
            var sec = Mathf.CeilToInt(time);
            _lastSec = sec;
            tmp.text = FormatTime(sec);
            ResetVisual();
        }

        public void UpdateTimer(float time)
        {
            var sec = Mathf.CeilToInt(time);
            if (sec != _lastSec)
            {
                _lastSec = sec;
                tmp.text = FormatTime(sec);
                PlayPulse().Forget();

                if (sec == 5)
                {
                    tmp.color = Color.red;
                }
            }

            if (sec <= 5)
            {
                ShakeTimer();
            }
        }

        public void ResetVisual()
        {
            rt.localScale = Vector3.one;
            rt.anchoredPosition = Vector2.zero;
            tmp.color = Color.white;
        }

        private void ShakeTimer()
        {
            var xPoint = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            var yPoint = Mathf.Cos(Time.time * shakeSpeed * 0.85f) * shakeAmount;
            rt.anchoredPosition = new Vector2(xPoint, yPoint);
        }

        private string FormatTime(int sec)
        {
            if (sec >= 60)
            {
                var minutes = sec / 60;
                var seconds = (sec % 60);
                return $"{minutes:0}:{seconds:00}";
            }
            else
            {
                var seconds = sec;
                if (seconds < 0) seconds = 0;
                return seconds.ToString();
            }
        }

        private async UniTaskVoid PlayPulse()
        {
            const float inDuration = 0.1f;
            const float duration = 0.2f;
            const float scale = 1.25f;

            var timer = 0f;
            var scaleDifference = scale - 1f;
            while (timer < inDuration)
            {
                timer += Time.deltaTime;
                var curScale = 1 + scaleDifference * TBMUtility.OutBack(timer, inDuration);
                rt.localScale = new Vector3(curScale, curScale, 1f);
                await UniTask.Yield(destroyCancellationToken);
            }

            timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                var curScale = 1 + scaleDifference * (1 - TBMUtility.InOutSine(timer, duration));
                rt.localScale = new Vector3(curScale, curScale, 1f);
                await UniTask.Yield(destroyCancellationToken);
            }
        }
    }
}