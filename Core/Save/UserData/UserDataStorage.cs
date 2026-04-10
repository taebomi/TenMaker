using TenMaker.Core.Data;
using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Core.Save
{
    public class UserDataStorage
    {
        public void Save(UserData userData)
        {
            PlayerPrefsUtility.SetBool(SaveKeys.TUTORIAL_COMPLETED, userData.tutorialCompleted);
            PlayerPrefs.SetString(SaveKeys.LAST_READ_NOTIFICATION_DATE, userData.lastReadNotificationDate);
            PlayerPrefsUtility.SetDateTime(SaveKeys.TERMS_CONSENT_DATE, userData.termsConsentDate);
            PlayerPrefsUtility.SetDateTime(SaveKeys.GDPR_CONSENT_DATE, userData.gdprConsentDate);
            PlayerPrefsUtility.SetBool(SaveKeys.GDPR_ADS_PERSONALIZATION, userData.gdprAdsPersonalization);
        }

        public UserData Load()
        {
            var playerData = new UserData
            {
                tutorialCompleted = PlayerPrefsUtility.GetBool(SaveKeys.TUTORIAL_COMPLETED, false),
                lastReadNotificationDate = PlayerPrefsUtility.GetString(SaveKeys.LAST_READ_NOTIFICATION_DATE),
                termsConsentDate = PlayerPrefsUtility.GetDateTime(SaveKeys.TERMS_CONSENT_DATE),
                gdprConsentDate = PlayerPrefsUtility.GetDateTime(SaveKeys.GDPR_CONSENT_DATE),
                gdprAdsPersonalization = PlayerPrefsUtility.GetBool(SaveKeys.GDPR_ADS_PERSONALIZATION, true)
            };

            return playerData;
        }
    }
}