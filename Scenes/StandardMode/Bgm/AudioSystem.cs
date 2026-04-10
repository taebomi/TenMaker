using FMODUnity;
using TenMaker.Core.Audio;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TenMaker.StandardMode
{
    public class AudioSystem : MonoBehaviour
    {
        [field: SerializeField] public BgmTimerMonitor BgmTimerMonitor { get; private set; }

        [SerializeField] private EventReference[] bgmRefs;
        [SerializeField] private EventReference gameOverSfxRef;

        private void OnEnable()
        {
            BgmTimerMonitor.ThirtySecondsLeft += OnTimer30SecLeft;
            BgmTimerMonitor.FiveSecondsLeft += OnTimer5SecLeft;
        }

        private void OnDisable()
        {
            BgmTimerMonitor.ThirtySecondsLeft -= OnTimer30SecLeft;
            BgmTimerMonitor.FiveSecondsLeft -= OnTimer5SecLeft;
        }

        public void ResetSystem()
        {
            BgmTimerMonitor.ResetMonitor();
        }

        public void PlayRandomBgm()
        {
            TMAudioManager.Instance.PlayBgm(bgmRefs[Random.Range(0, bgmRefs.Length)]);
        }

        public void StopBgm()
        {
            TMAudioManager.Instance.TransitionBgmPitch(1f);
            TMAudioManager.Instance.StopBgm();
        }

        public void PlayGameOverSfx()
        {
            TMAudioManager.Instance.PlaySfx(gameOverSfxRef);
        }

        private void OnTimer30SecLeft()
        {
            TMAudioManager.Instance.TransitionBgmPitch(1.15f);
        }

        private void OnTimer5SecLeft()
        {
            TMAudioManager.Instance.TransitionBgmPitch(1.35f);
        }
    }
}