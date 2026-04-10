using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Services.UGS.Multiplay;
using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Core
{
    public static class ServiceBootstrapper
    {
        public static async UniTask WaitForCompletedAsync()
        {
            if (TMGameManager.Instance == null) throw new System.Exception("Game Service is null");
            if (TMGameManager.Instance.IsInitialized) return;

            await TMGameManager.Instance.WaitForInitializationCompletedAsync();
        }
    }
}