using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TenMaker.Core.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace TenMaker.MainScene.HowToPlay
{
    public class HowToPlayUI : MonoBehaviour
    {
        private const int FIRST_STEP = 0;
        private const int LAST_STEP = 2;

        [SerializeField] private RectTransform panelRt;
        [SerializeField] private Animator previewAnimator;
        [SerializeField] private NextButton nextButton;
        [SerializeField] private TMP_Text descriptionTmp;
        [SerializeField] private StepViewer stepViewer;
        [SerializeField] private LocalizedString[] descriptionKeys;

        private int _currentStep;

        private UniTaskCompletionSource _hideTcs;

        public void Show()
        {
            _currentStep = FIRST_STEP;
            gameObject.SetActive(true);
            UpdateStep(_currentStep);
        }

        public void OnNextButtonClicked()
        {
            _currentStep++;
            if (_currentStep > LAST_STEP)
            {
                gameObject.SetActive(false);
                _hideTcs?.TrySetResult();
                TMUserDataRepository.Instance.CompleteTutorial();

                return;
            }

            UpdateStep(_currentStep);
        }

        private void UpdateStep(int step)
        {
            if (step is < FIRST_STEP or > LAST_STEP)
            {
                throw new ArgumentOutOfRangeException();
            }

            previewAnimator.Play($"Step{step + 1}");
            descriptionTmp.text = descriptionKeys[step].GetLocalizedString();
            stepViewer.ShowStep(step);
            nextButton.UpdateText(step);
            ShowAsync().Forget();
        }

        private async UniTaskVoid ShowAsync()
        {
            nextButton.gameObject.SetActive(false);
            await panelRt.DOScaleX(1f, 0.125f).From(0f).SetEase(Ease.OutBack).Play()
                .AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, destroyCancellationToken);
            nextButton.gameObject.SetActive(true);
        }

        public UniTask WaitForHide()
        {
            _hideTcs = new UniTaskCompletionSource();
            destroyCancellationToken.Register(CancelHideTcs);
            return _hideTcs.Task;
        }

        private void CancelHideTcs()
        {
            if (_hideTcs != null)
            {
                _hideTcs.TrySetResult();
                _hideTcs = null;
            }
        }
    }
}