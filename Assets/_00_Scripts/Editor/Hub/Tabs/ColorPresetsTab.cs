using Sirenix.OdinInspector;
using TenMaker.Utility;
using TenMaker.Utility.SO;
using UnityEngine;

namespace TenMaker.Editor.Hub
{
    public class ColorPresetsTab
    {
        [OnValueChanged(nameof(OnUiColorPresetChanged))]
        [InlineEditor] public ColorsSO uiColorPreset;

        private readonly TenMakerHubDataSO _hubData;

        public ColorPresetsTab(TenMakerHubDataSO dataSO)
        {
            _hubData = dataSO;
        }

        private void OnUiColorPresetChanged()
        {
            _hubData.uiColorPresetPath = uiColorPreset;
        }
    }
}