using TenMaker.Core.Audio;
using TenMaker.Core.Data;
using TenMaker.Utility;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace TenMaker.Core.Save
{
    public class SettingsDataStorage
    {
        public void Save(SettingsData data)
        {
            SaveGeneralSettings(data.generalSettingsData);
            SaveAudioSettings(data.audioSettingsData);
        }

        public SettingsData Load()
        {
            var settingsData = new SettingsData
            {
                generalSettingsData = LoadGeneralSettings(),
                audioSettingsData = LoadAudioSettings()
            };
            return settingsData;
        }

        #region General

        private GeneralSettingsData LoadGeneralSettings()
        {
            var defaultLocaleCode = LocalizationSettings.SelectedLocale.Identifier.Code;
            var localeCode = PlayerPrefsUtility.GetString(SaveKeys.LANGUAGE, defaultLocaleCode);
            return new GeneralSettingsData(localeCode);
        }

        private void SaveGeneralSettings(GeneralSettingsData generalSettingsData)
        {
            PlayerPrefs.SetString(SaveKeys.LANGUAGE, generalSettingsData.localeCode);
        }

        #endregion

        #region Audio

        private AudioSettingsData LoadAudioSettings()
        {
            var bgmMuted = PlayerPrefsUtility.GetBool(SaveKeys.BGM_MUTED, false);
            var bgmVolume = PlayerPrefsUtility.GetFloat(SaveKeys.BGM_VOLUME, 1f);
            var bgmConfig = new AudioChannelConfig(bgmMuted, bgmVolume);

            var sfxMuted = PlayerPrefsUtility.GetBool(SaveKeys.SFX_MUTED, false);
            var sfxVolume = PlayerPrefsUtility.GetFloat(SaveKeys.SFX_VOLUME, 1f);
            var sfxConfig = new AudioChannelConfig(sfxMuted, sfxVolume);

            return new AudioSettingsData(bgmConfig, sfxConfig);
        }

        private void SaveAudioSettings(AudioSettingsData audioSettingsData)
        {
            PlayerPrefsUtility.SetBool(SaveKeys.BGM_MUTED, audioSettingsData.bgmConfig.muted);
            PlayerPrefsUtility.SetFloat(SaveKeys.BGM_VOLUME, audioSettingsData.bgmConfig.volume);
            PlayerPrefsUtility.SetBool(SaveKeys.SFX_MUTED, audioSettingsData.sfxConfig.muted);
            PlayerPrefsUtility.SetFloat(SaveKeys.SFX_VOLUME, audioSettingsData.sfxConfig.volume);
        }

        #endregion
    }
}