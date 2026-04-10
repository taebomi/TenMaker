using System;
using DG.Tweening;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public class GridCursor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;

        [SerializeField] private float sizeAmplitude;
        [SerializeField] private float scaleAmplitude;
        [SerializeField] private float pulseDuration;
        [SerializeField] private Ease easeIn;
        [SerializeField] private Ease easeOut;

        private Sequence _sequence;
        private Vector3 _baseScale;
        private Vector2 _baseSize;
        private float _pulseValue;

        private float _alpha;

        private void Awake()
        {
            _baseScale = Vector3.one;
            _baseSize = sr.size;

            _sequence = DOTween.Sequence();
            _sequence.Append(DOTween.To(() => -1f, v => _pulseValue = v, 1f, pulseDuration * 0.5f)
                    .SetEase(easeIn))
                .Append(DOTween.To(() => 1f, v => _pulseValue = v, -1f, pulseDuration * 0.5f)).SetEase(easeOut)
                .SetLoops(-1, LoopType.Restart).Pause();
        }

        private void OnEnable()
        {
            _sequence.Restart();
        }

        private void Update()
        {
            var sizeFactor = 1 + _pulseValue * sizeAmplitude;
            var scaleFactor = 1 - _pulseValue * scaleAmplitude;

            sr.size = _baseSize * sizeFactor;
            transform.localScale = _baseScale * scaleFactor;
        }

        private void OnDisable()
        {
            _sequence.Pause();
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }

        public void SetAlpha(float alpha)
        {
            var color = sr.color;
            color.a = alpha;
            sr.color = color;
        }

        public void SetSize(Vector2 size)
        {
            _baseSize = size;
        }
    }
}