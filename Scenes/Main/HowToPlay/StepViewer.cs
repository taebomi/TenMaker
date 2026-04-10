using UnityEngine;
using UnityEngine.UI;

namespace TenMaker.MainScene.HowToPlay
{
    public class StepViewer : MonoBehaviour
    {
        [SerializeField] private RawImage[] images;
        [SerializeField] private Color stepColor;
        [SerializeField] private Color defaultColor;

        public void ShowStep(int step)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = i == step ? stepColor : defaultColor;
            }
        }
    }
}