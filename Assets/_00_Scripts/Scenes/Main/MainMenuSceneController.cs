using Cysharp.Threading.Tasks;
using TenMaker.Core.Data;
using TenMaker.Core.Scene;
using TenMaker.Core.UGS;
using TenMaker.MainScene.HowToPlay;
using UnityEngine;

namespace TenMaker.Scenes.Main
{
    public class MainMenuSceneController : BaseSceneController
    {
        #region static

        private static bool _isFirstEnter;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            _isFirstEnter = true;
        }

        #endregion

        public override string SceneName => SceneNames.MAIN_MENU;

        [SerializeField] private Authenticator authenticator;

        [SerializeField] private MainMenuUI mainMenuUI;
        [SerializeField] private HowToPlayUI howToPlayUI;

        protected override UniTask InitializeSceneAsync()
        {
            if (_isFirstEnter) mainMenuUI.Show();
            return UniTask.CompletedTask;
        }

        protected override async UniTask ProcessSceneAsync()
        {
            if (!_isFirstEnter) return;
            _isFirstEnter = false;

            // 초기 작업
            await authenticator.ProcessInitialSignInAsync(destroyCancellationToken);
            mainMenuUI.Show();
        }

        public void StartStandardMode()
        {
            if (!TMUserDataRepository.Instance.TutorialCompleted)
            {
                TutorialBeforeStartGameAsync().Forget();
                return;
            }

            TMSceneService.Instance.LoadScene(SceneNames.STANDARD_MODE);
        }

        private async UniTaskVoid TutorialBeforeStartGameAsync()
        {
            howToPlayUI.Show();
            await howToPlayUI.WaitForHide();
            TMSceneService.Instance.LoadScene(SceneNames.STANDARD_MODE);
        }
    }
}