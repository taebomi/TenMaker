using System.Collections.Generic;
using Unity.Services.Authentication;

namespace TenMaker.Services.UGS.Authentication
{
    public class SignInPayload
    {
        public List<Notification> Notifications;

        public SignInPayload()
        {
            Notifications = new List<Notification>();
        }

        public SignInPayload(List<Notification> notifications)
        {
            Notifications = notifications;
        }
    }
}