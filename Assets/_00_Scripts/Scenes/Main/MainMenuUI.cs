using Cysharp.Threading.Tasks;
using TenMaker.Core;
using TenMaker.Core.Localization;
using TenMaker.MainScene;
using TenMaker.MainScene.HowToPlay;
using TenMaker.MainScene.Settings;
using TenMaker.UI;
using UnityEngine;
using UnityEngine.Localization;

namespace TenMaker.Scenes.Main
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private MainMenuSceneController mainMenuMenuSceneController;

        [SerializeField] private SettingsUI settingsUI;
        [SerializeField] private MultiplayUI multiplayUI;
        [SerializeField] private HowToPlayUI howToPlayUI;
        [SerializeField] private LeaderboardUI leaderboardUI;
        [SerializeField] private GameObject creditsButton;
        [SerializeField] private GameObject quitButton;

        [SerializeField] private GameObject accountSettingsButton;

        [SerializeField] private LocalizedString needSignedInLocalizedStr;

        [SerializeField] private LocalizedString quitKey;

        [SerializeField] private MessageBox messageBox;


        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        #region Button Callback

        public void OnStartButtonClicked()
        {
            mainMenuMenuSceneController.StartStandardMode();
        }

        public void OnMultiplayButtonClicked()
        {
            Hide();
            multiplayUI.Show();
        }

        public void OnSettingsButtonClicked()
        {
            settingsUI.Show();
        }

        public void OnQuitButtonClicked()
        {
            CheckQuitAsync().Forget();
            return;

            async UniTask CheckQuitAsync()
            {
                var message = LocalizedStrings.System.quit_game.GetLocalizedString();
                var request = MessageBoxRequest.Create()
                    .SetMessage(message)
                    .SetButtonPreset(MessageBoxButtonPreset.YesNo)
                    .Build();
                var result = await messageBox.Show(request);
                if (result.Button == MessageBoxButtonType.Yes)
                {
                    TMGameManager.Instance.QuitGame();
                }
            }
        }

        public void OnTutorialButtonClicked()
        {
            howToPlayUI.Show();
        }

        public void OnLeaderboardsButtonClicked()
        {
            leaderboardUI.Show();
        }

        #endregion
    }
}