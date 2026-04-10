using Cysharp.Threading.Tasks;
using TenMaker.Core.Scene;
using TenMaker.Gameplay.Score.Multiplay.Data;
using TenMaker.Services.UGS.Multiplay;
using TMPro;
using UnityEngine;

namespace TenMaker.Scenes.DuelMode
{
    public class DuelResultUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text localScoreTmp;
        [SerializeField] private TMP_Text localResultTmp;
        [SerializeField] private TMP_Text opponentScoreTmp;
        [SerializeField] private TMP_Text opponentResultTmp;

        public void Show(ScoreDTO scoreDto, ulong localClientId)
        {
            int localScore = 0, opponentScore = 0;
            for (var i = 0; i < scoreDto.ClientIds.Length; i++)
            {
                if (scoreDto.ClientIds[i] == localClientId)
                    localScore = scoreDto.Scores[i];
                else
                    opponentScore = scoreDto.Scores[i];
            }

            localScoreTmp.text = localScore.ToString();
            opponentScoreTmp.text = opponentScore.ToString();

            string localResult, opponentResult;
            if (localScore > opponentScore)
            {
                localResult = "WIN";
                opponentResult = "LOSE";
            }
            else if (localScore < opponentScore)
            {
                localResult = "LOSE";
                opponentResult = "WIN";
            }
            else
            {
                localResult = "DRAW";
                opponentResult = "DRAW";
            }

            localResultTmp.text = localResult;
            opponentResultTmp.text = opponentResult;

            gameObject.SetActive(true);
        }

        public void OnExitButtonClicked() => OnExitButtonClickedAsync().Forget();

        private async UniTaskVoid OnExitButtonClickedAsync()
        {
            await TMMultiplayService.Instance.LeaveRoomAsync();
            TMSceneService.Instance.LoadScene(SceneNames.MAIN_MENU);
        }
    }
}