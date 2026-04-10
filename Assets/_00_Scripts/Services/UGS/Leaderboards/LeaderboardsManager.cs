using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Services.UGS.Authentication;
using TenMaker.Utility.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.Localization;

namespace TenMaker.Services.UGS.Leaderboards
{
    public class LeaderboardsManager : MonoBehaviour, ITMLeaderboardsService
    {
        public ILeaderboardsService Service { get; private set; }

        public void Initialize(ILeaderboardsService service)
        {
            Service = service;
        }

        public void Deinitialize()
        {
            if (Service == null) return;

            Service = null;
        }

        public async UniTask<(Result<LeaderboardPageData> rankerPageData, Result<LeaderboardPageData>
                leaderboardPageData)>
            GetLeaderboardAsync(string leaderboardId, int pageSize,
                int rankerSize, CancellationToken ct)
        {
            var (rankerPageData, leaderboardPageResult) = await UniTask.WhenAll(
                GetRankerAsync(leaderboardId, rankerSize, ct),
                GetPlayerPageAsync(leaderboardId, pageSize, ct));
            if (!leaderboardPageResult.IsSuccess)
            {
                leaderboardPageResult = await GetPageAsync(leaderboardId, 0, pageSize, ct);
            }
            
            return (rankerPageData, leaderboardPageResult);
        }

        public async UniTask<Result<LeaderboardPageData>> GetRankerAsync(string leaderboardId, int rankerSize,
            CancellationToken ct)
        {
            return await GetPageAsync(leaderboardId, 0, rankerSize, ct);
        }

        public async UniTask<Result<LeaderboardPageData>> GetPageAsync(string leaderboardId, int pageIndex,
            int pageSize, CancellationToken ct)
        {
            if (Service == null)
            {
                return Result<LeaderboardPageData>.Fail(ErrorCode.NOT_INITIALIZED);
            }

            LeaderboardScoresPage scoresPage = null;
            try
            {
                var option = new GetScoresOptions
                {
                    Limit = pageSize,
                    Offset = pageIndex * pageSize
                };
                scoresPage = await Service.GetScoresAsync(leaderboardId, option).AsUniTask()
                    .AttachExternalCancellation(ct);
            }
            catch (LeaderboardsValidationException ex)
            {
                return Result<LeaderboardPageData>.Fail(ex.ErrorCode, message: ex.Reason.ToString());
            }
            catch (LeaderboardsException ex)
            {
                return Result<LeaderboardPageData>.Fail(ex.ErrorCode, message: ex.Reason.ToString());
            }
            catch (OperationCanceledException)
            {
                return Result<LeaderboardPageData>.Fail(ErrorCode.CANCELLED);
            }
            catch
            {
                return Result<LeaderboardPageData>.Fail(ErrorCode.UNKNOWN);
            }

            var pageEntries = scoresPage.Results;
            var totalPage = (scoresPage.Total + pageSize - 1) / pageSize;
            var playerEntry = pageEntries.Find(entry => entry.PlayerId == TMAuthenticationService.Instance.PlayerId);
            var pageData = new LeaderboardPageData(pageEntries, pageIndex, totalPage, playerEntry);
            return Result<LeaderboardPageData>.Success(pageData);
        }

        public async UniTask<Result<LeaderboardPageData>> GetPlayerPageAsync(string leaderboardId, int pageSize,
            CancellationToken ct)
        {
            if (Service == null)
            {
                return Result<LeaderboardPageData>.Fail(ErrorCode.NOT_INITIALIZED);
            }

            List<LeaderboardEntry> leaderboardEntries = null;
            try
            {
                var option = new GetPlayerRangeOptions
                {
                    RangeLimit = pageSize - 1
                };
                var leaderboardScores = await Service.GetPlayerRangeAsync(leaderboardId, option).AsUniTask()
                    .AttachExternalCancellation(ct);
                leaderboardEntries = leaderboardScores.Results;
            }
            catch (LeaderboardsException ex) when (ex.Reason ==
                                                   LeaderboardsExceptionReason.ScoreSubmissionRequired)
            {
                return Result<LeaderboardPageData>.Fail(ErrorCode.Leaderboards.NO_ENTRY, message: ex.Reason.ToString());
            }
            catch (LeaderboardsException ex)
            {
                return Result<LeaderboardPageData>.Fail(ex.ErrorCode, message: ex.Reason.ToString());
            }
            catch (OperationCanceledException)
            {
                return Result<LeaderboardPageData>.Fail(ErrorCode.CANCELLED);
            }
            catch
            {
                return Result<LeaderboardPageData>.Fail(ErrorCode.UNKNOWN);
            }

            var playerId = TMAuthenticationService.Instance.PlayerId;
            var playerEntry = leaderboardEntries.Find(entry => entry.PlayerId == playerId);
            if (playerEntry == null)
            {
                return Result<LeaderboardPageData>.Fail(ErrorCode.Leaderboards.NO_ENTRY);
            }

            var playerRank = playerEntry.Rank;
            var pageIndex = (playerRank - 1) / pageSize;
            var startRank = pageIndex * pageSize;
            var endRank = startRank + pageSize - 1;

            var entries = leaderboardEntries
                .Where(entry => entry.Rank >= startRank && entry.Rank <= endRank)
                .OrderBy(entry => entry.Rank)
                .ToList();

            var pageData = new LeaderboardPageData(entries, pageIndex, pageSize, playerEntry);
            return Result<LeaderboardPageData>.Success(pageData);
        }
    }
}