using UnityEngine;

namespace TenMaker.Utility.SO
{
    [CreateAssetMenu(fileName = "Colors SO", menuName = "TBM/SO/Colors")]
    public class ColorsSO : ScriptableObject
    {
        public Color[] colors;
    }
}