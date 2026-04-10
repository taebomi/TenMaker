using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Player.Multiplay;
using UnityEngine;

namespace TenMaker.Gameplay
{
    [CreateAssetMenu(fileName = "CheckeredColorPalette SO", menuName = "Ten Maker/Customization/Tile/Checkered",
        order = 0)]
    public class CheckeredColorPaletteSO : SerializedScriptableObject
    {
        [field: SerializeField] public Dictionary<PlayerColor, CheckeredColorPair> ColorPairs { get; private set; }
    }

    [Serializable]
    public struct CheckeredColorPair
    {
        public Color baseColor;
        public Color accentColor;
    }
}