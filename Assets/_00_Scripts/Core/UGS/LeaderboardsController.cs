using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Services.UGS;
using TenMaker.Services.UGS.Leaderboards;
using TenMaker.Utility.Core;
using UnityEngine;

namespace TenMaker.Core.UGS.Leaderboards
{
    public class LeaderboardsController : MonoBehaviour
    {
        private int _curPageIndex;
        private int _totalPageCount;
        private int _pageSize;

        public async UniTask<(Result<LeaderboardPageData> ranker, Result<LeaderboardPageData> page)>
            GetLeaderboardAsync(int rankerSize, int pageSize, CancellationToken ct)
        {
            _pageSize = pageSize;
            var result = await TMLeaderboardsService.Instance.GetLeaderboardAsync(
                LeaderboardsIds.STANDARD_MODE, pageSize, rankerSize, ct);

            if (result.leaderboardPageData.IsSuccess)
            {
                _curPageIndex = result.leaderboardPageData.Value.PageIndex;
                _totalPageCount = result.leaderboardPageData.Value.TotalPages;
            }

            return (result.rankerPageData, result.leaderboardPageData);
        }

        public UniTask<Result<LeaderboardPageData>> ChangePageAsync(int delta, CancellationToken ct)
            => FetchPageAsync(_curPageIndex + delta, ct);

        public UniTask<Result<LeaderboardPageData>> FirstPageAsync(CancellationToken ct)
            => FetchPageAsync(0, ct);

        public UniTask<Result<LeaderboardPageData>> LastPageAsync(CancellationToken ct)
            => FetchPageAsync(_totalPageCount - 1, ct);

        private async UniTask<Result<LeaderboardPageData>> FetchPageAsync(int page, CancellationToken ct)
        {
            if (page < 0 || page >= _totalPageCount)
                return Result<LeaderboardPageData>.Fail(ErrorCode.INVALID);

            var result = await TMLeaderboardsService.Instance.GetPageAsync(
                LeaderboardsIds.STANDARD_MODE, page, _pageSize, ct);

            if (result.IsSuccess)
            {
                _curPageIndex = page;
                _totalPageCount = result.Value.TotalPages;
            }

            return result;
        }
    }
}
