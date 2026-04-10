using System;
using UnityEngine;

namespace TenMaker.UI
{
    [CreateAssetMenu(fileName = "ColorPresetsSO", menuName = "Ten Maker/UI/Color/Presets")]
    public class ColorPaletteSO : ScriptableObject
    {
        [field: SerializeField] public TextColorPreset TextPresets { get; private set; }
    }

    [Serializable]
    public class TextColorPreset
    {
        public Color black;
    }

    [Serializable]
    public class ButtonColorPreset
    {
        public ButtonColorSet positive;
        public ButtonColorSet negative;
        public ButtonColorSet destructive;
        public ButtonColorSet neutral;
    }

    [Serializable]
    public class ButtonColorSet
    {
        public Color background;
        public Color text;
    }
}