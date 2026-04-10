using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text bestScoreTmp;
        [SerializeField] private TMP_Text curScoreTmp;

        private const float PULSE_DURATION = 0.15f;
        [SerializeField] private Color pulseColor = new(1f, 0.8f, 0.2f);
        [SerializeField] private float pulseSize = 1.5f;

        private Sequence _pulseSequence;

        private void Awake()
        {
            _pulseSequence = DOTween.Sequence()
                .Append(curScoreTmp.transform.DOScale(1f, PULSE_DURATION).From(pulseSize).SetEase(Ease.InQuad))
                .Join(curScoreTmp.DOColor(Color.white, PULSE_DURATION).From(pulseColor).SetEase(Ease.InQuad))
                .SetAutoKill(false);
            _pulseSequence.Complete();
        }

        private void OnDisable()
        {
            _pulseSequence.Kill();
        }

        public void SetCurScore(int curScore)
        {
            curScoreTmp.text = curScore.ToString("000");
            _pulseSequence.Restart();
        }

        public void Setup()
        {
            curScoreTmp.text = "000";
        }
    }
}