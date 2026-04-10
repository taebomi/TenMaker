using System.Collections.Generic;
using TenMaker.Core.Save;
using UnityEngine.Localization;

namespace TenMaker.Core.Localization
{
    public interface ITMLocalizationManager
    {
        Locale CurrentLocale { get; }
        string CurrentLocaleCode { get; }
        List<Locale> AvailableLocales { get; }

        void SetLocale(Locale locale);
    }
}