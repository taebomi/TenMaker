using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Utility;

namespace TenMaker.Gameplay.Timer
{
    public class Timer
    {
        // public properties
        public double TotalTime { get; private set; }
        public double StartedTime { get; private set; }
        public double RemainedTime { get; private set; }

        public bool IsRunning { get; private set; }

        // events
        public event Action<double> TimerUpdated;
        public event Action TimerFinished;

        // private fields
        private readonly ITimeSource _timeSource;
        private CancellationTokenSource _timerCts;

        public Timer(ITimeSource timeSource)
        {
            _timeSource = timeSource;
        }

        public void Start(double totalTime, double startedTime, CancellationToken ct)
        {
            if (IsRunning)
            {
                TBMLog.HeaderWarning($"Timer is already running. Stopping it first.");
                Stop();
            }

            IsRunning = true;
            StartedTime = startedTime;
            RemainedTime = TotalTime = totalTime;

            _timerCts?.Dispose();
            _timerCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            UpdateAsync(_timerCts.Token).Forget();
        }

        public void Stop()
        {
            if (IsRunning is false) return;

            IsRunning = false;
            if (_timerCts != null)
            {
                _timerCts.Cancel();
                _timerCts.Dispose();
                _timerCts = null;
            }
        }

        private async UniTask UpdateAsync(CancellationToken ct)
        {
            while (RemainedTime > 0f && IsRunning && ct.IsCancellationRequested is false)
            {
                var currentTime = _timeSource.GetTime();
                var elapsedTime = currentTime - StartedTime;
                RemainedTime = TotalTime - elapsedTime;

                if (RemainedTime <= 0f)
                {
                    break;
                }

                TimerUpdated?.Invoke(RemainedTime);
                await UniTask.Yield(ct);
            }

            IsRunning = false;
            RemainedTime = 0;
            TimerUpdated?.Invoke(0);
            TimerFinished?.Invoke();
        }
    }
}