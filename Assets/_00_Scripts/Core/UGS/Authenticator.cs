using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Core.Localization;
using TenMaker.Services.UGS.Authentication;
using TenMaker.UI;
using Unity.Services.Authentication;
using UnityEngine;

namespace TenMaker.Core.UGS
{
    public class Authenticator : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private LoadingOverlay loadingOverlay;
        [SerializeField] private MessageBox messageBox;

        public bool IsSignedIn => TMAuthenticationService.Instance.IsSignedIn;

        /// <summary>
        /// 게임 시작 시 첫 로그인 시도 용도
        /// </summary>
        /// <param name="ct"></param>
        public async UniTask ProcessInitialSignInAsync(CancellationToken ct)
        {
            loadingOverlay.Show();
            var result = await TMAuthenticationService.Instance.SignInAsync(ct);
            loadingOverlay.Hide();
            if (!result.IsSuccess)
            {
                var message = LocalizedStrings.System.Authentication_SignIn_Failed.GetLocalizedString();
                ShowSignResultMessage(message);
            }
        }

        public async UniTask SignInAsGuestAsync(CancellationToken ct)
        {
            loadingOverlay.Show();
            var result = await TMAuthenticationService.Instance.SignInAsync(ct);
            loadingOverlay.Hide();


            // 결과 메시지 출력
            var message = result.IsSuccess
                ? LocalizedStrings.System.Authentication_SignIn_Success.GetLocalizedString()
                : LocalizedStrings.System.Authentication_SignIn_Failed.GetLocalizedString();
            ShowSignResultMessage(message);
        }

        public void SignOut()
        {
            var result = TMAuthenticationService.Instance.SignOut(false);

            var content = result.IsSuccess
                ? LocalizedStrings.System.Authentication_SignOut_Success
                : LocalizedStrings.System.Authentication_SignOut_Failed;
            ShowSignResultMessage(content.GetLocalizedString());
        }

        private void ShowSignResultMessage(string message)
        {
            messageBox.Show(MessageBoxRequest.Create()
                .SetMessage(message)
                .SetButtonPreset(MessageBoxButtonPreset.Confirm)
                .Build()).Forget();
        }

        /// <summary>
        /// DSA Notification UI에 출력 및 결과 저장
        /// </summary>
        private async UniTask ShowNotifications(List<Notification> notifications)
        {
            await UniTask.CompletedTask;
        }
    }
}