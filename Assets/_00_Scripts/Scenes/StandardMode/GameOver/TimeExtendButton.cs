using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TenMaker.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace TenMaker.StandardMode
{
    public class TimeExtendButton : MonoBehaviour
    {
        [BoxGroup("buttonPulse")]
        [SerializeField] private Button button;
        [BoxGroup("buttonPulse")]
        [SerializeField] private float pulseUpSize;
        [BoxGroup("buttonPulse")]
        [SerializeField] private float pulseDownSize;
        [BoxGroup("buttonPulse")]
        [SerializeField] private float pulseUpDuration;
        [BoxGroup("buttonPulse")]
        [SerializeField] private float pulseDownDuration;

        [BoxGroup("OuterPulse")]
        [SerializeField] private Image outerPulseImage;
        [BoxGroup("OuterPulse")]
        [SerializeField] private float outerPulseDuration;
        [BoxGroup("OuterPulse")]
        [SerializeField] private Vector2 outerPulseMinSize;
        [BoxGroup("OuterPulse")]
        [SerializeField] private Vector2 outerPulseMaxSize;
        [BoxGroup("OuterPulse")]
        [SerializeField] private float outerPulseStartPPUM;
        [BoxGroup("OuterPulse")]
        [SerializeField] private float outerPulseEndPPUM;

        [BoxGroup("Timer")]
        [SerializeField] private float timerDuration;
        [SerializeField] private Image timerImage;

        private Sequence _buttonPulseSequence;
        private Sequence _outerPulseSequence;

        private float _clickDeadlineRealTime;

        private DisableCancellationTokenProvider _disableCancellationTokenProvider;

        private void Awake()
        {
            var buttonRt = button.GetComponent<RectTransform>();
            _buttonPulseSequence = DOTween.Sequence()
                .Append(buttonRt.DOScale(pulseDownSize, pulseUpDuration).From(pulseUpSize).SetEase(Ease.OutBack))
                .Append(buttonRt.DOScale(1f, pulseDownDuration).From(pulseDownSize).SetEase(Ease.InQuad))
                .SetLoops(-1, LoopType.Restart);


            _outerPulseSequence = DOTween.Sequence()
                .Append(outerPulseImage.DOFade(0f, outerPulseDuration))
                .Join(outerPulseImage.rectTransform.DOSizeDelta(outerPulseMaxSize, outerPulseDuration)
                    .From(outerPulseMinSize))
                .Join(DOTween.To(() => outerPulseImage.pixelsPerUnitMultiplier,
                    x => outerPulseImage.pixelsPerUnitMultiplier = x,
                    outerPulseEndPPUM, outerPulseDuration).From(outerPulseStartPPUM))
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.OutQuad);

            _disableCancellationTokenProvider = this.GetDisableCancellationTokenProvider();
        }

        private void OnEnable()
        {
            _buttonPulseSequence.Restart();
            _outerPulseSequence.Restart();
        }

        private void OnDisable()
        {
            _buttonPulseSequence.Pause();
            _outerPulseSequence.Pause();
        }

        private void OnDestroy()
        {
            _buttonPulseSequence.Kill();
            _outerPulseSequence.Kill();
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void UpdateRemainedTime(float ratio)
        {
            timerImage.fillAmount = Mathf.Clamp01(ratio);
        }
    }
}