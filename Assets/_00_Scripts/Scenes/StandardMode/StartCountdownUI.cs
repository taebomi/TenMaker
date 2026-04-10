using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class StartCountdownUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text countdownTmp;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }


        public async UniTask CountdownAsync()
        {
            gameObject.SetActive(true);
            countdownTmp.text = "3";
            await UniTask.Delay(1000, cancellationToken: destroyCancellationToken);
            countdownTmp.text = "2";
            await UniTask.Delay(1000, cancellationToken: destroyCancellationToken);
            countdownTmp.text = "1";
            await UniTask.Delay(1000, cancellationToken: destroyCancellationToken);
            gameObject.SetActive(false);
        }
    }
}