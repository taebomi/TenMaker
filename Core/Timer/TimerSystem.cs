using System;
using TenMaker.RankMode;
using UnityEngine;

namespace TenMaker.Core
{
    public class TimerSystem : MonoBehaviour
    {
        [field: SerializeField] public Timer Timer { get; private set; }
        [SerializeField] private TimerUI timerUI;

        private void OnEnable()
        {
            Timer.TimerUpdated += timerUI.UpdateTimer;
        }

        private void OnDisable()
        {
            Timer.TimerUpdated -= timerUI.UpdateTimer;
        }

        public void SetTimer(float time)
        {
            Timer.SetTimer(time);
            timerUI.Setup(time);
        }

        public void ResetTimer()
        {
            Timer.ResetTimer();
            timerUI.UpdateTimer(Timer.RemainedTime);
        }

        public void HandleGameStart()
        {
            Timer.StartTimer();
        }

        public void StopTimer()
        {
            Timer.StopTimer();
            timerUI.ResetVisual();
        }
    }
}