using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TenMaker.Services.UGS.Leaderboards;
using TenMaker.StandardMode;
using TenMaker.Utility;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class LeaderboardsController : MonoBehaviour
    {
        public const string LEADERBOARD_ID = LeaderboardsIds.STANDARD_MODE;

        [SerializeField] private LeaderboardUI leaderboardUI;

        public List<LeaderboardEntry> RankerEntries { get; private set; }
        public LeaderboardEntry PlayerEntry { get; private set; }


        public async UniTask SetupAsync()
        {
            var result = await GetRankerScoresAsync();
            result = await GetPlayerEntryAsync() && result;
            if (result is false)
            {
                leaderboardUI.SetType(LeaderboardUI.ShowType.Error);
                return;
            }
        }

        [Button]
        public async UniTask HandleGameOverAsync(int score)
        {
            leaderboardUI.SetType(LeaderboardUI.ShowType.Loading);
            var result = await AddScoreAsync(score);
            if (result is false)
            {
                leaderboardUI.SetType(LeaderboardUI.ShowType.Error);
                return;
            }

            result = await GetRankerScoresAsync();
            if (result is false)
            {
                leaderboardUI.SetType(LeaderboardUI.ShowType.Error);
                return;
            }

            if (PlayerEntry != null)
            {
                leaderboardUI.SetPlayerEntry(PlayerEntry);
            }
            else
            {
                leaderboardUI.SetType(LeaderboardUI.ShowType.Error);
                return;
            }

            if (RankerEntries != null)
            {
                leaderboardUI.SetRankerEntry(RankerEntries);
            }
            else
            {
                leaderboardUI.SetType(LeaderboardUI.ShowType.Error);
                return;
            }


            leaderboardUI.SetType(LeaderboardUI.ShowType.Leaderboard);
        }

        private async UniTask<bool> AddScoreAsync(int score)
        {
            try
            {
                // var entry = await TMLeaderboardsService.Instance.AddScoreAsync(LEADERBOARD_ID, score);
                // PlayerEntry = entry;
                return true;
            }
            catch (LeaderboardsValidationException ex)
            {
                TBMLog.SimpleLog($"# {nameof(LeaderboardsController)} - Failed to add score: {ex.Message}");
                return false;
            }
        }

        public async UniTask<bool> GetPlayerEntryAsync()
        {
            try
            {
                // PlayerEntry = await TMLeaderboardsService.Instance.GetPlayerScoreAsync(LEADERBOARD_ID);
                return true;
            }
            catch (LeaderboardsValidationException ex)
            {
                TBMLog.SimpleLog($"# {nameof(LeaderboardsController)} - Failed to get player entry: {ex.Message}");
                PlayerEntry = null;
                return false;
            }
        }

        public async UniTask<bool> GetRankerScoresAsync()
        {
            try
            {
                // var page = await TMLeaderboardsService.Instance.GetScoresAsync(LEADERBOARD_ID, new GetScoresOptions()
                // {
                //     Limit = 3, Offset = 0
                // });
                // RankerEntries = page.Results;
                return true;
            }
            catch (LeaderboardsValidationException ex)
            {
                TBMLog.SimpleLog($"# {nameof(LeaderboardsController)} - Failed to get ranker scores: {ex.Message}");
                RankerEntries = null;
                return false;
            }
        }
    }
}