using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Player.Multiplay;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public class CheckeredBackground : CellBackground
    {
        [SerializeField] private CheckeredColorPaletteSO colorPalette;

        [SerializeField] private SpriteRenderer sr;

        private bool _isBaseColor;
        // 색상 설정
        // 자신의 컬러 타입 결정
        
        public void SetType(bool isBase)
        {
            _isBaseColor = isBase;
        }

        public override void SetPlayerArea(PlayerColor color)
        {
            var colorPair = colorPalette.ColorPairs[color];
            sr.color = _isBaseColor ? colorPair.baseColor : colorPair.accentColor;
        }
    }
}