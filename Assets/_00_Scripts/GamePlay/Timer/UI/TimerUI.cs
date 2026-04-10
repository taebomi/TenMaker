using System;
using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Gameplay.Timer
{
    public class TimerUI : MonoBehaviour
    {
        private const float CRITICAL_TIME = 5f;
        [SerializeField] private TimerTmp timerTmp;
        [SerializeField] private TimerBar timerBar;

        private bool _isCriticalTime;
        private float _totalTime;

        private void Update()
        {
            if (_isCriticalTime)
            {
                timerTmp.ShakePosition();
                timerBar.ShakeYPosition();
            }
        }

        public void SetTotalTime(float totalTime)
        {
            if (totalTime == 0)
            {
                TBMLog.HeaderLog($"Total Time Cannot set 0");
                totalTime = 120;
            }

            _totalTime = totalTime;
            timerTmp.Setup(totalTime);
            timerBar.ResetVisual();
        }

        public void UpdateRemainedTime(float remainedTime)
        {
            var isCriticalTime = remainedTime <= CRITICAL_TIME;
            if (_isCriticalTime != isCriticalTime)
            {
                _isCriticalTime = isCriticalTime;
                if (isCriticalTime)
                {
                    timerTmp.SetColor(Color.red);
                }
                else
                {
                    timerTmp.SetColor(Color.white);
                }
            }

            timerTmp.UpdateTimer(remainedTime);
            timerBar.UpdateRatio(remainedTime / _totalTime);
        }

        public void ResetVisual()
        {
            timerTmp.ResetVisual();
            timerBar.ResetVisual();
            _isCriticalTime = false;
        }
    }
}