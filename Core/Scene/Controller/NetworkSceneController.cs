using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Core.Scene
{
    [DefaultExecutionOrder(-1)]
    public abstract class NetworkSceneController : NetworkBehaviour, ISceneController
    {
        public abstract string SceneName { get; }

        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            IsInitialized = false;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            InitializeAsync().Forget();
        }

        private async UniTaskVoid InitializeAsync()
        {
            // 게임 초기화 대기
            await ServiceBootstrapper.WaitForCompletedAsync();

            // 씬 초기화
            await InitializeSceneAsync();
            IsInitialized = true;
            await TMSceneService.Instance.WaitForSceneReady();

            // 씬 진행
            await ProcessSceneAsync();
        }

        protected abstract UniTask InitializeSceneAsync();
        protected abstract UniTask ProcessSceneAsync();
    }
}