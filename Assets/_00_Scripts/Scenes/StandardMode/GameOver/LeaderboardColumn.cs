using TMPro;
using UnityEngine;

namespace TenMaker.StandardMode
{
    public class LeaderboardColumn : MonoBehaviour
    {
        [SerializeField] private TMP_Text rankTmp;
        [SerializeField] private TMP_Text playerNameTmp;
        [SerializeField] private TMP_Text scoreTmp;

        public void Setup(int rank, string playerName, int score)
        {
            rankTmp.text = (rank + 1).ToString();
            playerNameTmp.text = playerName;
            scoreTmp.text = score.ToString();
        }

        public void SetNull()
        {
            rankTmp.text = "";
            playerNameTmp.text = "";
            scoreTmp.text = "";
        }
    }
}