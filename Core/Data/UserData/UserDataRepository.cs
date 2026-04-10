using System;
using TenMaker.Core.Save;
using UnityEngine;

namespace TenMaker.Core.Data
{
    public class UserDataRepository : MonoBehaviour, IUserDataRepository
    {
        // tutorial
        public bool TutorialCompleted { get; private set; }
        // authentication
        public string LastDsaNotificationDate { get; private set; }
        #region Save/Load

        public void SetData(UserData data)
        {
            TutorialCompleted = data.tutorialCompleted;
            LastDsaNotificationDate = data.lastReadNotificationDate;
        }

        public UserData GetData()
        {
            var userData = new UserData
            {
                tutorialCompleted = TutorialCompleted,
                lastReadNotificationDate = LastDsaNotificationDate,
            };
            return userData;
        }

        #endregion


        // Terms Of Service
        public void CompleteTutorial()
        {
            TutorialCompleted = true;
        }

        public void ReadNotification(string date)
        {
            LastDsaNotificationDate = date;
        }
    }
}