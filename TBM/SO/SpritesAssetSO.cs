using UnityEngine;

namespace TenMaker.Utility.SO
{
    [CreateAssetMenu(fileName = "SpritesAsset", menuName = "TBM/SO/Asset/Sprites")]
    public class SpritesAssetSO : ScriptableObject
    {
        public Sprite[] sprites;
    }
}