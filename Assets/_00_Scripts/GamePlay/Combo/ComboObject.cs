using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace TenMaker.Gameplay.Combo
{
    public class ComboObject : MonoBehaviour
    {
        private TMP_Text _tmp;

        private IObjectPool<ComboObject> _pool;

        [SerializeField] private ComboObjectPresetSO presets;

        private void Awake()
        {
            _tmp = GetComponent<TMP_Text>();
        }

        public void Initialize(IObjectPool<ComboObject> pool)
        {
            _pool = pool;
        }

        public void Setup(int combo, Vector3 position)
        {
            gameObject.SetActive(true);
            var preset = presets.GetPreset(combo);
            _tmp.color = preset.color;
            _tmp.fontSize = preset.size;
            _tmp.text = string.Format(preset.format, combo);
            transform.position = position;
            PlayAnimationAsync(destroyCancellationToken).Forget();
        }

        private async UniTaskVoid PlayAnimationAsync(CancellationToken ct)
        {
            var sequence = DOTween.Sequence()
                .Append(transform.DOMoveY(0f, 0.1f).SetEase(Ease.InSine).From(-0.5f, true, true).SetRelative(true))
                .Join(_tmp.DOFade(1f, 0.1f).SetEase(Ease.InSine).From(0f))
                .Join(_tmp.DOScale(1f, 0.1f).From(1.5f).SetEase(Ease.InSine))
                .Append(transform.DOMoveY(0.1f, 1f).SetRelative().SetEase(Ease.Linear))
                .Append(transform.DOMoveY(1.5f, 0.15f).SetRelative().SetEase(Ease.OutQuad))
                .Join(_tmp.DOFade(0f, 0.15f).SetEase(Ease.OutQuad))
                .Join(_tmp.transform.DOScaleX(0f, 0.15f).SetEase(Ease.OutQuad));
            await sequence.Play().AwaitForComplete(TweenCancelBehaviour.KillAndCancelAwait, ct);

            if (_pool == null)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                _pool.Release(this);
            }
        }
    }
}