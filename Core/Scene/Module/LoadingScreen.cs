using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TenMaker.Core.Scene
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        public async UniTask FadeInAsync()
        {
            canvasGroup.gameObject.SetActive(true);
            await canvasGroup.DOFade(1f, 0.125f).From(0f).SetEase(Ease.OutQuad).Play()
                .AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, destroyCancellationToken);
        }
        
        public async UniTask FadeOutAsync()
        {
            await canvasGroup.DOFade(0f, 0.125f).From(1f).SetEase(Ease.OutQuad).Play()
                .AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, destroyCancellationToken);
            canvasGroup.gameObject.SetActive(false);
        }
    }
}