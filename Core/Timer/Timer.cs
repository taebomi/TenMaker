using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TenMaker.Core
{
    public class Timer : MonoBehaviour
    {
        public float TotalTime { get; private set; }
        public float RemainedTime { get; private set; }

        public bool IsRunning { get; private set; } // start / stop
        public bool IsPaused { get; private set; } // pause / resume

        private float _lastRealTime;

        private CancellationTokenSource _runningCts;

        public event Action<float> TimerUpdated;
        public event Action TimerFinished;

        private void Awake()
        {
            TotalTime = 0f;
            IsRunning = false;
            IsPaused = false;
        }


        private void OnDisable()
        {
            _runningCts?.Cancel();
        }

        private void OnDestroy()
        {
            _runningCts?.Dispose();
        }

        public void StartTimer()
        {
            if (IsRunning) return;
            IsRunning = true;
            IsPaused = false;

            _runningCts?.Dispose();
            _runningCts = new CancellationTokenSource();
            TimerAsync(_runningCts.Token).Forget();
        }

        public void StopTimer()
        {
            if (IsRunning is false) return;

            IsRunning = false;
            IsPaused = false;
            _runningCts?.Cancel();
        }

        public void SetTimer(float totalTime)
        {
            TotalTime = totalTime;
            RemainedTime = TotalTime;
        }

        public void ResetTimer()
        {
            RemainedTime = TotalTime;
        }

        public void PauseTimer()
        {
            IsPaused = true;
        }

        public void ResumeTimer()
        {
            IsPaused = false;
        }

        private async UniTaskVoid TimerAsync(CancellationToken ct)
        {
            RemainedTime = TotalTime;
            _lastRealTime = Time.realtimeSinceStartup;
            while (RemainedTime > 0f && IsRunning && ct.IsCancellationRequested is false)
            {
                if (IsPaused)
                {
                    await UniTask.Yield(ct);
                    continue;
                }

                var now = Time.realtimeSinceStartup;
                var delta = now - _lastRealTime;
                RemainedTime -= delta;

                if (RemainedTime <= 0)
                {
                    OnTimerFinished();
                    break;
                }

                TimerUpdated?.Invoke(RemainedTime);
                _lastRealTime = now;
                await UniTask.Yield(ct);
            }
        }

        private void OnTimerFinished()
        {
            IsRunning = false;
            IsPaused = false;
            RemainedTime = TotalTime;
            TimerUpdated?.Invoke(0f);
            TimerFinished?.Invoke();
        }
    }
}