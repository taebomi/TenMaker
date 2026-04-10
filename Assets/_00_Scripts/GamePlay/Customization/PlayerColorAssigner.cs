using System;
using System.Collections.Generic;
using TenMaker.Gameplay.Player.Multiplay;
using Random = UnityEngine.Random;

namespace TenMaker.Gameplay.Customization
{
    public class PlayerColorAssigner
    {
        private List<PlayerColor> _colorList;
        private List<PlayerColor> _availableColors;

        public PlayerColorAssigner()
        {
            _colorList = new List<PlayerColor>((PlayerColor[])Enum.GetValues(typeof(PlayerColor)));
            _availableColors = new List<PlayerColor>(_colorList.GetRange(1, _colorList.Count - 1));
        }

        public PlayerColor GetRandomColor()
        {
            if (_availableColors.Count == 0)
            {
                return (PlayerColor)Random.Range(0, Enum.GetValues(typeof(PlayerColor)).Length);
            }

            var randomIndex = Random.Range(0, _availableColors.Count);
            var randomColor = _availableColors[randomIndex];
            _availableColors.RemoveAt(randomIndex);
            return randomColor;
        }
    }
}