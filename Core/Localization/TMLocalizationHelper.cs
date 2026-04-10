using TenMaker.Core.Save;
using TenMaker.Utility;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TenMaker.Core.Localization
{
    public static class TMLocalizationHelper
    {
        public const string KO_KR = "ko-KR"; // 0
        public const string EN_US = "en-US"; // 1
        public const string JA_JP = "ja-JP"; // 2


        public static Locale GetLocale(string localeCode)
        {
            var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
            if (locale == null)
            {
                TBMLog.HeaderError($"{localeCode} not exists in AvailableLocales.");
                locale = LocalizationSettings.AvailableLocales.GetLocale(EN_US);
            }

            return locale;
        }

        public static Locale GetLocale(int localeIndex)
        {
            return GetLocale(IndexToLocaleCode(localeIndex));
        }

        public static int LocaleCodeToIndex(string localeCode)
        {
            switch (localeCode)
            {
                case KO_KR:
                    return 0;
                case EN_US:
                    return 1;
                case JA_JP:
                    return 2;
                default:
                    TBMLog.HeaderWarning($"{localeCode} not exists.");
                    return 1;
            }
        }

        public static string IndexToLocaleCode(int index)
        {
            switch (index)
            {
                case 0:
                    return KO_KR;
                case 1:
                    return EN_US;
                case 2:
                    return JA_JP;
                default:
                    TBMLog.HeaderWarning($"{index} not exists.");
                    return EN_US;
            }
        }
    }
}