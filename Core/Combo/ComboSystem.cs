using System;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;

namespace TenMaker.Core
{
    public class ComboSystem : MonoBehaviour
    {
        public int Combo { get; private set; }
        public int HighCombo { get; private set; }
        public bool IsCombo { get; private set; }

        private float _comboTimer;

        [SerializeField] private ComboDurationSO comboDurationSO;

        private void Awake()
        {
            Combo = 0;
            HighCombo = 0;
            _comboTimer = 0f;
        }

        public void AddCombo()
        {
            Combo++;
            HighCombo = Mathf.Max(Combo, HighCombo);
            var comboDurationIndex = Mathf.Clamp(Combo, 1, comboDurationSO.Length);
            _comboTimer = comboDurationSO[comboDurationIndex - 1];

            if (IsCombo) return;
            ComboAsync().Forget();
        }

        private async UniTaskVoid ComboAsync()
        {
            IsCombo = true;
            while (_comboTimer > 0f && destroyCancellationToken.IsCancellationRequested is false)
            {
                _comboTimer -= Time.deltaTime;
                await UniTask.Yield(destroyCancellationToken);
            }

            Combo = 0;
            IsCombo = false;
        }
    }
}