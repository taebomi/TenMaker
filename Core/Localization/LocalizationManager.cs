using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TenMaker.Core.Localization
{
    public class LocalizationManager : MonoBehaviour, ITMLocalizationManager
    {
        public Locale CurrentLocale => LocalizationSettings.SelectedLocale;
        public string CurrentLocaleCode => CurrentLocale.Identifier.Code;
        public List<Locale> AvailableLocales => LocalizationSettings.AvailableLocales.Locales;

        #region Initialization

        public async UniTask InitializeAsync()
        {
            await LocalizationSettings.InitializationOperation;

            TMLocalizationManager.Initialize(this);
        }

        public void Deinitialize()
        {
            TMLocalizationManager.Deinitialize(this);
        }

        #endregion

        public void SetLocale(Locale locale)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
    }
}