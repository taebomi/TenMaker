using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Core.Data;
using TenMaker.Utility;
using TenMaker.Utility.Core;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace TenMaker.Services.UGS.Authentication
{
    public class AuthenticationManager : MonoBehaviour, ITMAuthenticationService
    {
        public IAuthenticationService Service { get; private set; }

        public string PlayerId => Service.PlayerId;
        public string PlayerName => Service.PlayerName;

        public bool IsSignedIn => Service.IsSignedIn;

        public void Initialize(IAuthenticationService service)
        {
            Service = service;

            AddAuthenticationCallbacks();
        }

        public void Deinitialize()
        {
            if (Service == null) return;

            RemoveAuthenticationCallbacks();
            Service = null;
        }

        public void ApplySaveData()
        {
            PullStoredNotificationDate();
        }

        /// <returns>실패 시 payload null 체크 후 notification 확인필요.</returns>
        public async UniTask<Result<SignInPayload>> SignInAsync(CancellationToken ct)
        {
            if (Service == null)
            {
                TBMLog.HeaderWarning($"Service Not Initialized.");
                return Result<SignInPayload>.Fail(TMAuthenticationErrorCode.NOT_INITIALIZED, new SignInPayload());
            }

            try
            {
                TBMLog.SimpleLog($"Sign in started.");
                await Service.SignInAnonymouslyAsync().AsUniTask().AttachExternalCancellation(ct);
            }
            catch (AuthenticationException ex)
            {
                TBMLog.SimpleWarning($"ErrorCode[{ex.ErrorCode}]");
                return Result<SignInPayload>.Fail(ex.ErrorCode, new SignInPayload(ex.Notifications));
            }
            catch (OperationCanceledException)
            {
                TBMLog.SimpleWarning($"User Cancelled");
                return Result<SignInPayload>.Fail(TMAuthenticationErrorCode.USER_CANCELLED, new SignInPayload());
            }
            catch
            {
                TBMLog.SimpleWarning($"Error");
                return Result<SignInPayload>.Fail(0, new SignInPayload());
            }

            var notificationResult = await GetNotificationsAsync(ct);
            var payload = new SignInPayload(notificationResult.Value);
            return Result<SignInPayload>.Success(payload);
        }

        public Result SignOut(bool removeCredentials)
        {
            if (Service == null)
            {
                TBMLog.HeaderWarning($"Service Not Initialized.");
                return Result.Fail(TMAuthenticationErrorCode.NOT_INITIALIZED);
            }

            TBMLog.HeaderLog($"Sign out with remove Credentials[{removeCredentials}]");
            Service.SignOut(removeCredentials);
            return Result.Success();
        }

        #region DSA Notification

        private long _storedNotificationDate;

        /// <summary>
        /// 해당 Notification 읽음 처리 
        /// </summary>
        public void SetNotificationRead(Notification notification)
        {
            var notificationDate = ParseNotificationCreatedAt(notification.CreatedAt);
            if (notificationDate <= _storedNotificationDate) return;

            _storedNotificationDate = notificationDate;
            TMUserDataRepository.Instance.ReadNotification(notification.CreatedAt);
            TBMLog.HeaderLog($"Notification Id[{notification.Id}], Created At[{notification.CreatedAt}] Read");
        }

        /// <returns>실패 시 errorCode와 빈 리스트 반환</returns>
        private async UniTask<Result<List<Notification>>> GetNotificationsAsync(CancellationToken ct)
        {
            var lastNotificationDate = ParseNotificationCreatedAt(Service.LastNotificationDate);
            if (lastNotificationDate <= _storedNotificationDate)
            {
                return Result<List<Notification>>.Success(new List<Notification>());
            }

            try
            {
                var notifications = await Service.GetNotificationsAsync().AsUniTask()
                    .AttachExternalCancellation(ct);
                return Result<List<Notification>>.Success(notifications);
            }
            catch (AuthenticationException ex)
            {
                return Result<List<Notification>>.Fail(ex.ErrorCode, new List<Notification>());
            }
            catch (OperationCanceledException)
            {
                return Result<List<Notification>>.Fail(TMAuthenticationErrorCode.USER_CANCELLED,
                    new List<Notification>());
            }
            catch
            {
                return Result<List<Notification>>.Fail(CommonErrorCodes.Unknown, new List<Notification>());
            }
        }

        /// <summary>
        /// 저장된 notification date를 불러옴
        /// </summary>
        private void PullStoredNotificationDate()
        {
            _storedNotificationDate = ParseNotificationCreatedAt(TMUserDataRepository.Instance.LastDsaNotificationDate);
        }

        /// <summary>
        /// DSA Notification Created At string을 long으로 변환
        /// </summary>
        /// <returns>parse 불가 시 0 반환</returns>
        private long ParseNotificationCreatedAt(string createdAt)
        {
            return long.TryParse(createdAt, out var date) ? date : 0;
        }

        #endregion

        private void AddAuthenticationCallbacks()
        {
            Service.SignedIn += OnSignedIn;
            Service.SignedOut += OnSignedOut;
            Service.SignInFailed += OnSignInFailed;
            Service.Expired += OnSignInExpired;
        }

        private void RemoveAuthenticationCallbacks()
        {
            Service.SignedIn -= OnSignedIn;
            Service.SignedOut -= OnSignedOut;
            Service.SignInFailed -= OnSignInFailed;
            Service.Expired -= OnSignInExpired;
        }

        private void OnSignedIn()
        {
            TBMLog.HeaderLog($"Signed in successfully.");
        }

        private void OnSignedOut()
        {
            TBMLog.HeaderLog("Signed out successfully.");
        }

        /// <inheritdoc cref="IAuthenticationService.SignInFailed"/>
        private void OnSignInFailed(RequestFailedException requestFailedException)
        {
            TBMLog.HeaderWarning($"Sign in failed: {requestFailedException}");
            if (requestFailedException is AuthenticationException authException)
            {
                TBMLog.SimpleWarning($"Authentication error: {authException}");
            }
            else
            {
                TBMLog.SimpleWarning($"Request failed: {requestFailedException}");
            }
        }

        private void OnSignInExpired()
        {
            TBMLog.HeaderWarning("Sign in expired.");
        }
    }
}