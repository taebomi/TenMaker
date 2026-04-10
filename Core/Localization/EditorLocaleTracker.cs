#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization;

namespace TenMaker.Localization
{
    [InitializeOnLoad]
    public static class EditorLocaleTracker
    {
        public static Locale CurLocale { get; private set; }

        static EditorLocaleTracker()
        {
            CurLocale = LocalizationEditorSettings.ActiveLocalizationSettings.GetSelectedLocale();
            LocalizationEditorSettings.ActiveLocalizationSettings.OnSelectedLocaleChanged -= OnSelectedLocaleChanged;
            LocalizationEditorSettings.ActiveLocalizationSettings.OnSelectedLocaleChanged += OnSelectedLocaleChanged;
        }

        private static void OnSelectedLocaleChanged(Locale locale)
        {
            CurLocale = locale;
        }
    }
}

#endif