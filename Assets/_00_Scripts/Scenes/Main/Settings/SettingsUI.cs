using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using TenMaker.Core.Audio;
using TenMaker.Core.Data;
using TenMaker.Core.Localization;
using TenMaker.Core.Save;
using TenMaker.Core.UGS;
using TenMaker.Scenes.Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenMaker.MainScene.Settings
{
    public class SettingsUI : MonoBehaviour
    {
        // General Tab
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private LanguageItem[] languageItems;

        [Serializable]
        private struct LanguageItem
        {
            public string localeCode;
            public Sprite icon;
        }

        // Audio Tab
        [SerializeField] private Toggle bgmToggle;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Toggle sfxToggle;
        [SerializeField] private Slider sfxSlider;

        // Account Tab
        [SerializeField] private Authenticator authenticator;
        [SerializeField] private GameObject signInButton;
        [SerializeField] private GameObject signOutButton;

        private SettingsData _originalData;

        private void Awake()
        {
            InitializeGeneralTab();
        }

        public void Show()
        {
            _originalData = TMSettingsDataRepository.Instance.GetData();
            SetupGeneralTab();
            SetupAudioTab(_originalData.audioSettingsData);
            SetupAccountTab();
            gameObject.SetActive(true);
        }

        public void OnBackButtonClicked()
        {
            gameObject.SetActive(false);
        }


        #region General Tab

        private void InitializeGeneralTab()
        {
            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(languageItems.Select(item => item.icon).ToList());
            languageDropdown.onValueChanged.RemoveAllListeners();
            languageDropdown.onValueChanged.AddListener(OnLanguageDropdownChanged);
        }

        public void SetupGeneralTab()
        {
            var localeCode = TMLocalizationManager.Instance.CurrentLocaleCode;
            var index = 0;
            for (var i = 0; i < languageItems.Length; i++)
            {
                if (languageItems[i].localeCode != localeCode) continue;

                index = i;
                break;
            }

            languageDropdown.SetValueWithoutNotify(index);
            // var locales = TMLocalizationManager.Instance.AvailableLocales;
            // languageDropdown.ClearOptions();
            // var dropdownOptions = locales.Select(locale => locale.LocaleName).ToList();
            // languageDropdown.AddOptions(dropdownOptions);
            // languageDropdown.SetValueWithoutNotify(locales.IndexOf(TMLocalizationManager.Instance.CurrentLocale));
        }

        public void OnLanguageDropdownChanged(int index)
        {
            var selectedLocale = TMLocalizationManager.Instance.AvailableLocales[index];
            TMLocalizationManager.Instance.SetLocale(selectedLocale);
        }

        #endregion

        #region Audio Tab

        private void SetupAudioTab(AudioSettingsData data)
        {
            var bgmConfig = data.bgmConfig;
            bgmToggle.SetIsOnWithoutNotify(!bgmConfig.muted);
            bgmSlider.SetValueWithoutNotify(bgmConfig.volume);
            bgmSlider.gameObject.SetActive(!bgmConfig.muted);

            var sfxConfig = data.sfxConfig;
            sfxToggle.SetIsOnWithoutNotify(!sfxConfig.muted);
            sfxSlider.SetValueWithoutNotify(sfxConfig.volume);
            sfxSlider.gameObject.SetActive(!sfxConfig.muted);
        }

        public void OnBgmToggled(bool value)
        {
            bgmSlider.gameObject.SetActive(value);
            TMAudioManager.Instance.SetBgmMuted(!value);
        }

        public void OnBgmVolumeChanged(float value)
        {
            TMAudioManager.Instance.SetBgmVolume(value);
        }

        public void OnSfxToggled(bool value)
        {
            sfxSlider.gameObject.SetActive(value);
            TMAudioManager.Instance.SetSfxMuted(!value);
        }

        public void OnSfxVolumeChanged(float value)
        {
            TMAudioManager.Instance.SetSfxVolume(value);
        }

        #endregion

        #region Account Tab

        private void SetupAccountTab()
        {
            if (authenticator.IsSignedIn)
            {
                signInButton.SetActive(false);
                signOutButton.SetActive(true);
            }
            else
            {
                signInButton.SetActive(true);
                signOutButton.SetActive(false);
            }
        }

        private void RefreshAccountTab() => SetupAccountTab();

        public void OnSignInButtonClicked() => SignIn().Forget();

        private async UniTask SignIn()
        {
            await authenticator.SignInAsGuestAsync(destroyCancellationToken);
            RefreshAccountTab();
        }

        public void OnSignOutButtonClicked() => SignOut();

        private void SignOut()
        {
            authenticator.SignOut();
            RefreshAccountTab();
        }

        #endregion
    }
}