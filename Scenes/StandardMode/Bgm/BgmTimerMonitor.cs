using System;
using UnityEngine;

namespace TenMaker.StandardMode
{
    public class BgmTimerMonitor : MonoBehaviour
    {
        private bool _thirtySecTriggered;
        private bool _fiveSecTriggered;

        public event Action ThirtySecondsLeft;
        public event Action FiveSecondsLeft;

        public void ResetMonitor()
        {
            _fiveSecTriggered = false;
            _thirtySecTriggered = false;
        }

        public void OnTimerUpdated(float remainedTime)
        {
            if (_fiveSecTriggered is false && remainedTime <= 5f)
            {
                _fiveSecTriggered = true;
                _thirtySecTriggered = true;
                FiveSecondsLeft?.Invoke();
            }
            else if (_thirtySecTriggered is false && remainedTime <= 30f)
            {
                _thirtySecTriggered = true;
                ThirtySecondsLeft?.Invoke();
            }
        }
    }
}