using System;
using TenMaker.Core.Save;

namespace TenMaker.Core.Data
{
    public interface IUserDataRepository : IRepository<UserData>
    {
        public bool TutorialCompleted { get; }
        public string LastDsaNotificationDate { get; } // 유니티 공식에서 string으로 사용
        public void CompleteTutorial();
        public void ReadNotification(string createdAt);

    }
}