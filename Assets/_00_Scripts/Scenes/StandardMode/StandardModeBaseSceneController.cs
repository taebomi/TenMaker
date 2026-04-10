using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TenMaker.Core;
using TenMaker.Core.Scene;
using TenMaker.Core.Visualizer;
using TenMaker.StandardMode;
using TenMaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace TenMaker.RankMode
{
    public class StandardModeBaseSceneController : BaseSceneController
    {
        public override string SceneName => SceneNames.STANDARD_MODE;

        [SerializeField] private RankModeConfigSO config;

        [Header("Objects")]
        [SerializeField] private Player player;
        [SerializeField] private Camera mainCam;

        [Header("Modules")]
        [SerializeField] private AudioSystem audioSystem;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private ScoreSystem scoreSystem;
        [SerializeField] private TimerSystem timerSystem;
        [SerializeField] private RegionView regionView;

        [Header("UI")]
        [SerializeField] private StartCountdownUI startCountdownUI;
        [SerializeField] private GameOverUI gameOverUI;
        [SerializeField] private QuickMenuUI quickMenuUI;

        [SerializeField] private LeaderboardsController leaderboardsController;


#if UNITY_EDITOR
        [SerializeField] private bool skipCountdown;
#endif

        public bool IsPlaying { get; private set; }


        private void OnDisable()
        {
            RemoveGamePlayCallbacks();
        }

        protected override UniTask InitializeSceneAsync()
        {
            Setup();
            Ready();
            ResetGamePlay();
            return UniTask.CompletedTask;
        }

        protected override UniTask ProcessSceneAsync()
        {
            StartGameAsync().Forget();
            return UniTask.CompletedTask;
        }


        private void Setup()
        {
            gridManager.Setup(config.col, config.row);
            gridManager.SetVisibility(false);

            timerSystem.SetTimer(config.time);
            player.Setup(mainCam, gridManager);
            gameOverUI.Setup(PlayData.BestScore);

            leaderboardsController.SetupAsync().Forget();
        }

        private void Ready()
        {
            DeactivateUI();

            audioSystem.StopBgm();
            audioSystem.ResetSystem();

            regionView.Disable();
        }

        private void ResetGamePlay()
        {
            gridManager.SetVisibility(false);
            gridManager.CreateNewGrid();
            scoreSystem.ResetScore();
            timerSystem.SetTimer(config.time);
        }

        private void DeactivateUI()
        {
            quickMenuUI.Hide();
            startCountdownUI.SetActive(false);
            gameOverUI.Hide();
        }


        private async UniTaskVoid StartGameAsync()
        {
            IsPlaying = true;
#if UNITY_EDITOR
            if (skipCountdown is false)
            {
                await startCountdownUI.CountdownAsync();
            }
#else
            await startCountdownUI.CountdownAsync();
#endif
            timerSystem.HandleGameStart();
            gridManager.SetVisibility(true);
            quickMenuUI.Show();
            player.SetInput(true);
            audioSystem.PlayRandomBgm();
            AddGamePlayCallbacks();
        }

        private void GameOver()
        {
            FinishGame();

            audioSystem.PlayGameOverSfx();

            int score = scoreSystem.Score;
            if (score > PlayData.BestScore)
            {
                PlayData.BestScore = score;
                gameOverUI.UpdateBestScore(score);
            }

            gameOverUI.Show(scoreSystem.Score);
            leaderboardsController.HandleGameOverAsync(score).Forget();
        }

        /// <summary>
        /// 게임 종료 시 호출
        /// GameOver와 Restart 시 공통 실행
        /// </summary>
        private void FinishGame(bool showAd = true)
        {
            IsPlaying = false;

            RemoveGamePlayCallbacks();
            audioSystem.StopBgm();
            timerSystem.StopTimer();

            player.SetInput(false);
            quickMenuUI.Hide();
        }

        /// <summary>
        /// 게임 오버 이후 
        /// </summary>
        public async UniTaskVoid ExtendGame(float extendedTime)
        {
            TBMLog.HeaderLog($"Extend Game");

            timerSystem.SetTimer(extendedTime);
            Ready();
            StartGameAsync().Forget();
            await UniTask.CompletedTask;
        }


        public void OnRestartBtnClicked()
        {
            if (IsPlaying)
            {
                FinishGame();
            }

            Ready();
            StartGameAsync().Forget();
        }

        public void OnHomeBtnClicked()
        {
            if (IsPlaying) FinishGame(false);

            TMSceneService.Instance.LoadScene(SceneNames.MAIN_MENU);
        }

        public void OnSolutionBtnClicked()
        {
            gameOverUI.Hide();
            quickMenuUI.Show();
            regionView.Enable();
        }

        private void AddGamePlayCallbacks()
        {
            timerSystem.Timer.TimerUpdated += audioSystem.BgmTimerMonitor.OnTimerUpdated;
            timerSystem.Timer.TimerFinished += GameOver;
            player.ClearHandler.AreaClearRequested += OnPlayerAreaSelected;
        }

        private void RemoveGamePlayCallbacks()
        {
            timerSystem.Timer.TimerUpdated -= audioSystem.BgmTimerMonitor.OnTimerUpdated;
            timerSystem.Timer.TimerFinished -= GameOver;
            player.ClearHandler.AreaClearRequested -= OnPlayerAreaSelected;
        }

        private void OnPlayerAreaSelected(Cell startCell, Cell endCell, List<Cell> clearedCells)
        {
            var clearedCount = gridManager.TryClearSelection(startCell, endCell, clearedCells);
            if (clearedCount is 0) return;

            scoreSystem.AddScore(clearedCount);
            player.HandleAreaCleared();
        }
    }
}