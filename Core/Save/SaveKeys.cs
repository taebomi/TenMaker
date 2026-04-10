namespace TenMaker.Core.Save
{
    public static class SaveKeys
    {
        // # Settings
        // ## General
        public const string LANGUAGE = "language";
        
        // ## Audio
        public const string BGM_MUTED = "bgm_muted";
        public const string BGM_VOLUME = "bgm_volume";
        public const string SFX_MUTED = "sfx_muted";
        public const string SFX_VOLUME = "sfx_volume";
        
        // # User Data
        // ## Tutorial
        public const string TUTORIAL_COMPLETED = "tutorial_completed";
        
        // ## Authentication
        public const string LAST_READ_NOTIFICATION_DATE = "last_read_notification_date";
        
        // ## Privacy
        public const string TERMS_CONSENT_DATE = "terms_consent_date";
        public const string GDPR_CONSENT_DATE = "gdpr_checked";
        public const string GDPR_ADS_PERSONALIZATION = "gdpr_ads_personalization";
        
    }
}