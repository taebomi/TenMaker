using System;
using Cysharp.Threading.Tasks;
using TenMaker.Services.UGS.Authentication;
using TenMaker.Services.UGS.Leaderboards;
using TenMaker.Services.UGS.Multiplay;
using TenMaker.Utility;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

namespace TenMaker.Services.UGS
{
    public class UGSManager : MonoBehaviour
    {
        [field: SerializeField] public AuthenticationManager AuthenticationManager { get; private set; }
        [field: SerializeField] public LeaderboardsManager LeaderboardsManager { get; private set; }
        [field: SerializeField] public MultiplayManager MultiplayManager { get; private set; }

        public async UniTask InitializeAsync()
        {
            TMAuthenticationService.Initialize(AuthenticationManager);
            TMLeaderboardsService.Initialize(LeaderboardsManager);
            TMMultiplayService.Initialize(MultiplayManager);

            var isSuccess = await InitializeUnityServicesAsync();
            if (isSuccess)
            {
                AuthenticationManager.Initialize(AuthenticationService.Instance);
                LeaderboardsManager.Initialize(LeaderboardsService.Instance);
                MultiplayManager.Initialize();
            }
        }

        public void Deinitialize()
        {
            AuthenticationManager.Deinitialize();
            LeaderboardsManager.Deinitialize();

            TMAuthenticationService.Deinitialize(AuthenticationManager);
            TMLeaderboardsService.Deinitialize(LeaderboardsManager);
            TMMultiplayService.Deinitialize(MultiplayManager);
        }
        
        public void ApplySaveData()
        {
            AuthenticationManager.ApplySaveData();
        }

        private async UniTask<bool> InitializeUnityServicesAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();
                return true;
            }
            catch (Exception ex)
            {
                TBMLog.HeaderError($"{ex.Message}");
                return false;
            }
        }
    }
}