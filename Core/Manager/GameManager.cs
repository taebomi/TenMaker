using Cysharp.Threading.Tasks;
using TenMaker.Core.Audio;
using TenMaker.Core.Input;
using TenMaker.Core.Localization;
using TenMaker.Core.Save;
using TenMaker.Core.Scene;
using TenMaker.Services.UGS;
using UnityEngine;

namespace TenMaker.Core
{
    [DefaultExecutionOrder(-2)]
    public class GameManager : MonoBehaviour, IGameManager
    {
        [field: SerializeField] public AudioManager AudioManager { get; private set; }
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public SaveManager SaveManager { get; private set; }
        [field: SerializeField] public LocalizationManager LocalizationManager { get; private set; }

        [field: SerializeField] public UGSManager UGSManager { get; private set; }

        [field: SerializeField] public SceneManager SceneManager { get; private set; }

        public bool IsInitialized { get; private set; }


        private void Awake()
        {
            if (TMGameManager.IsInstanceExist)
            {
                Destroy(gameObject);
                return;
            }

            TMGameManager.Initialize(this);
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (!TMGameManager.IsInstance(this)) return;

            InitializeAsync().Forget();
        }

        private void OnApplicationQuit() // OnDisable보다 먼저 실행됨
        {
            if (!TMGameManager.IsInstance(this)) return;
            if (IsInitialized is false) return;

            SaveManager.Save();
        }

        private void OnDisable()
        {
            if (!TMGameManager.IsInstance(this)) return;
            if (IsInitialized is false) return;

            Deinitialize();
            TMGameManager.Deinitialize(this);
        }

        private async UniTaskVoid InitializeAsync()
        {
            AudioManager.Initialize();
            SaveManager.Initialize();
            InputManager.Initialize();
            SceneManager.Initialize();
            await LocalizationManager.InitializeAsync();
            await UGSManager.InitializeAsync();


            // 세이브 데이터 로드
            SaveManager.Load();

            // 세이브 데이터 적용
            UGSManager.ApplySaveData();
            AudioManager.ApplySaveData();

            IsInitialized = true;
        }

        private void Deinitialize()
        {
            LocalizationManager.Deinitialize();
            UGSManager.Deinitialize();
            SceneManager.Deinitialize();
            InputManager.Deinitialize();
            SaveManager.Deinitialize();
            AudioManager.Deinitialize();

            IsInitialized = false;
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }

        public async UniTask WaitForInitializationCompletedAsync()
        {
            if (IsInitialized) return;

            await UniTask.WaitUntil(() => IsInitialized);
        }
    }
}