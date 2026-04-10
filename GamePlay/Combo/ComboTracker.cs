using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Core;
using TenMaker.Utility;

namespace TenMaker.Gameplay
{
    /// <summary>
    /// 플레이어 콤보
    /// </summary>
    public class ComboTracker
    {
        // Constants

        // Properties
        public int CurrentCombo { get; private set; } = 0;
        public int HighestCombo { get; private set; } = 0;

        // Private Variables
        private CancellationTokenSource _comboResetTimerCts;

        public void IncreaseCombo(CancellationToken externalToken)
        {
            CurrentCombo++;
            if (CurrentCombo > HighestCombo)
            {
                HighestCombo = CurrentCombo;
            }

            RestartTimer(externalToken);
        }

        private void RestartTimer(CancellationToken externalToken)
        {
            _comboResetTimerCts?.CancelAndDispose();
            _comboResetTimerCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            RestartTimerAsync(_comboResetTimerCts.Token).Forget();
        }

        private async UniTaskVoid RestartTimerAsync(CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(GameRule.COMBO_RESET_TIME), cancellationToken: ct);
            ResetCombo();
        }

        private void ResetCombo()
        {
            CurrentCombo = 0;
        }
    }
}