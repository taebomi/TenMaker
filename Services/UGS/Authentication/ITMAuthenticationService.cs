using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Utility.Core;
using Unity.Services.Authentication;

namespace TenMaker.Services.UGS.Authentication
{
    public interface ITMAuthenticationService
    {
        string PlayerId { get; }
        bool IsSignedIn { get; }
        
        UniTask<Result<SignInPayload>> SignInAsync(CancellationToken ct);
        Result SignOut(bool removeCredentials);


        // DSA Notification
        /// <summary>
        /// 해당 DSA Notification 읽음 처리
        /// </summary>
        void SetNotificationRead(Notification notification);
    }
}