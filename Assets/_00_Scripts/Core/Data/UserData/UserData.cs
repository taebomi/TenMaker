using System;

namespace TenMaker.Core.Data
{
    [Serializable]
    public class UserData
    {
        public bool tutorialCompleted;

        public string lastReadNotificationDate;

        public DateTime termsConsentDate = DateTime.MinValue;
        public DateTime gdprConsentDate = DateTime.MinValue;
        public bool gdprAdsPersonalization = true;
    }
}