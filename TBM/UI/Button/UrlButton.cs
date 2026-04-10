using UnityEngine;

namespace TenMaker.UI
{
    public class UrlButton : MonoBehaviour
    {
        [SerializeField] private string url;

        public void OpenUrl()
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("URL is empty or null.");
                return;
            }

            Application.OpenURL(url);
        }
    }
}