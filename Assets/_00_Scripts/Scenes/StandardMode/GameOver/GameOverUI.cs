using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Febucci.UI;
using Sirenix.OdinInspector;
using TenMaker.StandardMode;
using TenMaker.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TenMaker.RankMode
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private TextAnimator_TMP titleTextAnimator;
        [SerializeField] private TextAnimator_TMP newBestScoreTextAnimator;
        [SerializeField] private TMP_Text bestScoreTmp;
        [SerializeField] private GameObject newBestScore;
        [SerializeField] private TMP_Text scoreTmp;

        [BoxGroup("Time Extend")]
        [SerializeField] private TimeExtendButton timeExtendButton;
        [BoxGroup("Time Extend")]
        [SerializeField] private float timeExtendClickableDuration;
        [BoxGroup("Time Extend")]
        [SerializeField] private UnityEvent extendTimeEvent;

        [SerializeField] private UnityEvent solutionBtnClickedEvent;

        private bool _isShowing;
        private float _extendTimeButtonClickableRealTimeDeadline;

        private DisableCancellationTokenProvider _disableCancellationTokenProvider;

        private void Awake()
        {
            _disableCancellationTokenProvider = this.GetDisableCancellationTokenProvider();

            titleTextAnimator.SetText("<bounce a=0.35 f=0.64 w=0.25>GAME OVER</>");
            newBestScoreTextAnimator.SetText("<incr a=0.75 f=3 w=0>NEW!</>");
        }

        public void Setup(int bestScore)
        {
            bestScoreTmp.text = bestScore.ToString();
            gameObject.SetActive(false);
            newBestScore.SetActive(false);
            _isShowing = false;
        }


        public void Show(int score)
        {
            Show();
            scoreTmp.text = $"{score}";
        }

        private void Show()
        {
            if (_isShowing) return;

            _isShowing = true;
            gameObject.SetActive(true);
            ShowAsync().Forget();
        }

        public void Hide()
        {
            if (_isShowing is false) return;

            HideAsync().Forget();
        }

        public void UpdateBestScore(int value)
        {
            newBestScore.SetActive(true);
            bestScoreTmp.text = value.ToString();
        }

        private async UniTaskVoid ShowAsync()
        {
            canvasGroup.interactable = false;
            await canvasGroup.DOFade(1f, 0.25f).From(0f).SetEase(Ease.OutQuad).Play()
                .AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, destroyCancellationToken);
            canvasGroup.interactable = true;
        }

        private async UniTaskVoid HideAsync()
        {
            canvasGroup.interactable = false;
            await canvasGroup.DOFade(0f, 0.25f).From(1f).SetEase(Ease.OutQuad).Play()
                .AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, destroyCancellationToken);
            gameObject.SetActive(false);
            _isShowing = false;
        }

        private async UniTaskVoid TimeExtendButtonAsync(CancellationToken ct)
        {
            _extendTimeButtonClickableRealTimeDeadline = Time.realtimeSinceStartup + timeExtendClickableDuration;

            timeExtendButton.SetActive(true);
            while (Time.realtimeSinceStartup < _extendTimeButtonClickableRealTimeDeadline &&
                   ct.IsCancellationRequested is false)
            {
                var deltaTime = _extendTimeButtonClickableRealTimeDeadline - Time.realtimeSinceStartup;
                var ratio = deltaTime / timeExtendClickableDuration;
                timeExtendButton.UpdateRemainedTime(ratio);
                await UniTask.Yield(ct);
            }

            timeExtendButton.SetActive(false);
        }

        public void OnTimeExtendButtonClicked()
        {
            if (Time.realtimeSinceStartup > _extendTimeButtonClickableRealTimeDeadline) return;

            extendTimeEvent.Invoke();
        }

        public void OnSolutionBtnClicked()
        {
            solutionBtnClickedEvent.Invoke();
        }
    }
}