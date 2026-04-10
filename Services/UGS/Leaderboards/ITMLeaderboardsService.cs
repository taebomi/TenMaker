using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Utility.Core;

namespace TenMaker.Services.UGS.Leaderboards
{
    public interface ITMLeaderboardsService
    {
        UniTask<(Result<LeaderboardPageData> rankerPageData, Result<LeaderboardPageData> leaderboardPageData)>
            GetLeaderboardAsync(string leaderboardId, int pageIndex, int pageSize, CancellationToken ct);

        UniTask<Result<LeaderboardPageData>> GetPageAsync(string leaderboardId, int pageIndex, int pageSize,
            CancellationToken ct);


        UniTask<Result<LeaderboardPageData>> GetRankerAsync(string leaderboardId, int rankerSize,
            CancellationToken ct);

        UniTask<Result<LeaderboardPageData>> GetPlayerPageAsync(string leaderboardId, int pageSize,
            CancellationToken ct);
    }
}