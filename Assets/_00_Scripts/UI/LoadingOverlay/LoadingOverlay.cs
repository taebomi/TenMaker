using UnityEngine;

namespace TenMaker.UI
{
    public class LoadingOverlay : MonoBehaviour
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