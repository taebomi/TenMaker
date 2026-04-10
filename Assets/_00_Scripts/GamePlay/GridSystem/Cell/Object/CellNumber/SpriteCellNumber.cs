using TenMaker.Utility.SO;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public class SpriteCellNumber : CellNumber
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private SpritesAssetSO spriteAsset;

        public override void SetValue(int newValue)
        {
            sr.sprite = spriteAsset.sprites[newValue - 1];
        }
    }
}