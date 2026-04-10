using System.Collections.Generic;
using TenMaker.StandardMode;
using TenMaker.Utility;
using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TenMaker.MainScene
{
    public class LeaderboardContentPanel : MonoBehaviour
    {
        [SerializeField] private List<LeaderboardColumn> rankerColumns;
        [SerializeField] private List<LeaderboardColumn> pageColumns;

        [SerializeField] private GameObject firstButton;
        [SerializeField] private GameObject leftButton;
        [SerializeField] private TMP_Text pageInfoTmp;
        [SerializeField] private GameObject rightButton;
        [SerializeField] private GameObject lastButton;

        public int RankerColumnCount => rankerColumns.Count;
        public int PageColumnCount => pageColumns.Count;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void ShowRanker(List<LeaderboardEntry> entries)
        {
            PopulateColumns(rankerColumns, entries);
        }

        public void ShowPage(List<LeaderboardEntry> entries, int totalPage)
        {
            PopulateColumns(pageColumns, entries);
            SetPageInfo(entries, totalPage);
        }

        public void UpdateNavigate(int pageIndex, int totalPageCount)
        {
            if (pageIndex == 0)
            {
                firstButton.SetActive(false);
                leftButton.SetActive(false);
            }
            else
            {
                firstButton.SetActive(true);
                leftButton.SetActive(true);
            }
            
            if (pageIndex == totalPageCount - 1)
            {
                rightButton.SetActive(false);
                lastButton.SetActive(false);
            }
            else
            {
                rightButton.SetActive(true);
                lastButton.SetActive(true);
            }
        }

        private void PopulateColumns(IReadOnlyList<LeaderboardColumn> columns, IReadOnlyList<LeaderboardEntry> entries)
        {
            TBMLog.HeaderAssert(entries.Count <= columns.Count,
                $"Entries count[{entries.Count}] > Columns count[{columns.Count}]");

            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                columns[i].Setup(entry.Rank, entry.PlayerName, (int)entry.Score);
            }

            for (var i = entries.Count; i < columns.Count; i++)
            {
                columns[i].SetNull();
            }
        }

        private void SetPageInfo(IReadOnlyList<LeaderboardEntry> entries, int totalPage)
        {
            var currentPage = 1;
            if (entries.Count > 0)
            {
                currentPage = (entries[0].Rank - 1) / PageColumnCount + 1;
            }

            pageInfoTmp.text = $"{currentPage} / {totalPage}";
        }
    }
}