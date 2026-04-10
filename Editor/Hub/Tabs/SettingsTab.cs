using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Modules.Localization.Editor;
using TenMaker.Core;
using TenMaker.Core.Localization;
using TenMaker.Core.Save;
using TenMaker.Localization;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace TenMaker.Editor.Hub
{
    public class SettingsTab
    {
        [HorizontalGroup("Bgm", Title = "Bgm"), LabelText("Muted")]
        [SerializeField] private bool bgmMuted;
        [HorizontalGroup("Bgm"), LabelText("Volume")]
        [Range(0f, 1f)]
        [SerializeField] private float bgmVolume;
        [HorizontalGroup("Sfx", Title = "Sfx"), LabelText("Muted")]
        [SerializeField] private bool sfxMuted;
        [HorizontalGroup("Sfx", Title = "Sfx"), LabelText("Volume")]
        [Range(0f, 1f)]
        [SerializeField] private float sfxVolume;

        [LabelText("Locale")]
        [ValueDropdown("GetLocaleOptions")]
        [SerializeField] private string locale;

        private SettingsDataStorage _dataStorage;
        private SettingsData _data;

        public SettingsTab()
        {
            _dataStorage = new SettingsDataStorage();
            _data = _dataStorage.Load();
            Update();
        }

        private void Update()
        {
            var generalSettings = _data.generalSettingsData;
            locale = generalSettings.localeCode;

            var audioSettings = _data.audioSettingsData;
            bgmMuted = audioSettings.bgmConfig.muted;
            bgmVolume = audioSettings.bgmConfig.volume;
            sfxMuted = audioSettings.sfxConfig.muted;
            sfxVolume = audioSettings.sfxConfig.volume;
        }

        [Button("Clear", ButtonSizes.Large)]
        private void Clear()
        {
            PlayerPrefs.DeleteAll();
        }

        [ButtonGroup("SaveLoad"), Button("Load", ButtonSizes.Large)]
        private void Load()
        {
            _data = _dataStorage.Load();
            Update();
        }

        [ButtonGroup("SaveLoad"), Button("Save", ButtonSizes.Large)]
        private void Save()
        {
            // var generalSettings = new GeneralSettingsData();
            // generalSettings.localeCode = locale;
            //
            // var audioSettings = new AudioSettingsData();
            // audioSettings.bgmConfig.muted = bgmMuted;
            // audioSettings.bgmConfig.volume = bgmVolume;
            // audioSettings.sfxConfig.muted = sfxMuted;
            // audioSettings.sfxConfig.volume = sfxVolume;
            //
            // _data.generalSettingsData = generalSettings;
            // _data.audioSettingsData = audioSettings;
            // _dataStorage.Save(_data);
        }

        private string[] GetLocaleOptions()
        {
            return LocalizationSettings.AvailableLocales.Locales.Select(locale => locale.Identifier.Code).ToArray();
        }
    }
}