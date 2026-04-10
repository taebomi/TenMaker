using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Core.UGS.Leaderboards;
using TenMaker.Services.UGS;
using TenMaker.Services.UGS.Leaderboards;
using TenMaker.UI;
using TenMaker.Utility;
using TenMaker.Utility.Core;
using UnityEngine;

namespace TenMaker.MainScene
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] private LeaderboardsController controller;
        [SerializeField] private LeaderboardContentPanel contentPanel;
        [SerializeField] private GameObject errorPanel;
        [SerializeField] private YoyoAnimationImage loadingImage;

        private bool _hasShown;

        public void Show()
        {
            gameObject.SetActive(true);

            if (!_hasShown)
                GetLeaderboardAsync(destroyCancellationToken).Forget();
        }

        public void Hide() => gameObject.SetActive(false);
        public void OnDimmedBackgroundClicked() => Hide();

        // Button Events
        public void OnPageLeftButtonClicked()  => PageActionAsync(controller.ChangePageAsync(-1, destroyCancellationToken)).Forget();
        public void OnPageRightButtonClicked() => PageActionAsync(controller.ChangePageAsync(+1, destroyCancellationToken)).Forget();
        public void OnPageFirstButtonClicked() => PageActionAsync(controller.FirstPageAsync(destroyCancellationToken)).Forget();
        public void OnPageLastButtonClicked()  => PageActionAsync(controller.LastPageAsync(destroyCancellationToken)).Forget();
        public void OnRetryButtonClicked()     => GetLeaderboardAsync(destroyCancellationToken).Forget();

        private async UniTaskVoid GetLeaderboardAsync(CancellationToken ct)
        {
            SwitchState(State.Loading);
            var (rankerResult, pageResult) = await controller.GetLeaderboardAsync(
                contentPanel.RankerColumnCount, contentPanel.PageColumnCount, ct);

            if (!rankerResult.IsSuccess || !pageResult.IsSuccess)
            {
                SwitchState(State.Error);
                return;
            }

            _hasShown = true;
            contentPanel.ShowRanker(rankerResult.Value.Entries);
            contentPanel.ShowPage(pageResult.Value.Entries, pageResult.Value.TotalPages);
            contentPanel.UpdateNavigate(pageResult.Value.PageIndex, pageResult.Value.TotalPages);
            SwitchState(State.Leaderboard);
        }

        private async UniTaskVoid PageActionAsync(UniTask<Result<LeaderboardPageData>> task)
        {
            SwitchState(State.Loading);
            var result = await task;

            if (!result.IsSuccess)
            {
                if (result.ErrorCode != ErrorCode.INVALID)
                    SwitchState(State.Error);
                else
                    SwitchState(State.Leaderboard);
                return;
            }

            contentPanel.ShowPage(result.Value.Entries, result.Value.TotalPages);
            contentPanel.UpdateNavigate(result.Value.PageIndex, result.Value.TotalPages);
            SwitchState(State.Leaderboard);
        }

        private void SwitchState(State state)
        {
            contentPanel.SetActive(state == State.Leaderboard);
            errorPanel.SetActive(state == State.Error);
            loadingImage.gameObject.SetActive(state == State.Loading);
        }

        private enum State
        {
            Loading,
            Leaderboard,
            Error
        }
    }
}
