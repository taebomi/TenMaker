using UnityEngine;

namespace TenMaker.RankMode
{
    public class QuickMenuUI : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}