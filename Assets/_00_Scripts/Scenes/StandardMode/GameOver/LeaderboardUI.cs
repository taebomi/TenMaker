using System.Collections.Generic;
using System.Threading;
using TenMaker.UI;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace TenMaker.StandardMode
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] private LeaderboardColumn[] rankerColumns;
        [SerializeField] private LeaderboardColumn playerColumn;
        
        [SerializeField] private GameObject leaderboard;
        [SerializeField] private GameObject errorMessage;
        [SerializeField] private YoyoAnimationImage loadingImage;

        private CancellationTokenSource _loadingCts;
        

        public void SetPlayerEntry(LeaderboardEntry entry)
        {
            if (entry == null)
            {
                playerColumn.SetNull();
                return;
            }
            
            playerColumn.Setup(entry.Rank, entry.PlayerName, (int)entry.Score);
        }

        public void SetRankerEntry(List<LeaderboardEntry> entries)
        {
            for (int rank = 0; rank < 3; rank++)
            {
                if (rank >= entries.Count)
                {
                    rankerColumns[rank].SetNull();
                    continue;
                }
                
                var ranker = entries[rank];
                rankerColumns[rank].Setup(ranker.Rank, ranker.PlayerName, (int)ranker.Score);
            }
        }

        public void SetType(ShowType type)
        {
            leaderboard.SetActive(type == ShowType.Leaderboard);
            loadingImage.gameObject.SetActive(type == ShowType.Loading);
            errorMessage.SetActive(type == ShowType.Error);
        }

        public enum ShowType
        {
            Leaderboard,
            Loading,
            Error,
        }
    }
}