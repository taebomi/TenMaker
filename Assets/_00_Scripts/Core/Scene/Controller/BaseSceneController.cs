using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TenMaker.Core.Scene
{
    [DefaultExecutionOrder(-1)]
    public abstract class BaseSceneController : MonoBehaviour, ISceneController
    {
        public abstract string SceneName { get; }
        public bool IsInitialized { get; private set; }

        protected void Awake()
        {
            IsInitialized = false;
        }

        protected void Start()
        {
            InitializeAsync().Forget();
        }

        private async UniTaskVoid InitializeAsync()
        {
            // 게임 초기화 
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