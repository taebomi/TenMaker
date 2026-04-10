using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;

namespace TenMaker.Services.UGS.Leaderboards
{
    public class LeaderboardPageData
    {
        public List<LeaderboardEntry> Entries;
        public int PageIndex;
        public int TotalPages;

        public LeaderboardEntry PlayerEntry;

        public LeaderboardPageData(List<LeaderboardEntry> entries, int pageIndex, int totalPages, LeaderboardEntry playerEntry)
        {
            Entries = entries;
            PageIndex = pageIndex;
            TotalPages = totalPages;
            PlayerEntry = playerEntry;
        }
    }
}