using TMPro;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TimerTmp timerTmp;
        [SerializeField] private TimerBar timerBar;

        private float _totalTime;

        public void Setup(float time)
        {
            _totalTime = time;
            timerTmp.Setup(time);
            timerBar.Setup(time);
        }

        public void UpdateTimer(float time)
        {
            timerTmp.UpdateTimer(time);
            timerBar.UpdateTimer(time);
        }

        public void ResetVisual()
        {
            timerTmp.ResetVisual();
        }
    }
}