using System.Collections.Generic;
using DG.Tweening;
using TenMaker.Core;
using UnityEngine;

namespace TenMaker.Gameplay.Combo
{
    [CreateAssetMenu(fileName = "Combo Object Preset SO", menuName = "Ten Maker/Combo/Preset")]
    public class ComboObjectPresetSO : ScriptableObject
    {

        [SerializeField] private List<ComboObjectPreset> presets;

        public ComboObjectPreset GetPreset(int combo)
        {
            var index = combo - GameRule.MINIMUM_COMBO;
            index = Mathf.Clamp(index, 0, presets.Count - 1);
            return presets[index];
        }
    }
}