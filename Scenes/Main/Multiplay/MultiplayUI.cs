using Cysharp.Threading.Tasks;
using TenMaker.Core.Localization;
using TenMaker.Services.UGS;
using TenMaker.Services.UGS.Multiplay;
using TenMaker.UI;
using TMPro;
using UnityEngine;

namespace TenMaker.Scenes.Main
{
    public class MultiplayUI : MonoBehaviour
    {
        [SerializeField] private MainMenuUI mainMenuUI;
        [SerializeField] private MessageBox messageBox;

        [SerializeField] private GameObject multiplayMenu;
        [SerializeField] private GameObject waitingRoom;
        [SerializeField] private TMP_Text joinCodeTmp;
        [SerializeField] private UnityEngine.UI.Button leaveRoomButton;

        [SerializeField] private LoadingOverlay loadingOverlay;

        public void Show()
        {
            gameObject.SetActive(true);
            multiplayMenu.SetActive(true);
            waitingRoom.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnBackButtonClicked()
        {
            Hide();
            mainMenuUI.Show();
        }

        // ── 방 만들기 ──────────────────────────────────────

        public void OnCreateRoomButtonClicked() => OnCreateRoomButtonClickedAsync().Forget();

        private async UniTaskVoid OnCreateRoomButtonClickedAsync()
        {
            loadingOverlay.Show();
            var result = await TMMultiplayService.Instance.CreateRoomAsync(destroyCancellationToken);
            loadingOverlay.Hide();

            if (result.ErrorCode == ErrorCode.CANCELLED) return;

            if (!result.IsSuccess)
            {
                var msg = LocalizedStrings.MainScene.Multiplay_CreateRoom_Failed.GetLocalizedString();
                await messageBox.Show(MessageBoxRequest.Create()
                    .SetMessage(msg)
                    .SetButtonPreset(MessageBoxButtonPreset.Confirm)
                    .Build());
                return;
            }

            joinCodeTmp.text = result.Value.JoinCode;
            EnterWaitingRoom();
        }

        // ── 방 입장 ────────────────────────────────────────

        public void OnJoinRoomButtonClicked() => OnJoinRoomButtonClickedAsync().Forget();

        private async UniTaskVoid OnJoinRoomButtonClickedAsync()
        {
            var inputMsg = LocalizedStrings.MainScene.Multiplay_JoinRoom_MessageBox.GetLocalizedString();
            var inputResult = await messageBox.Show(MessageBoxRequest.Create()
                .SetMessage(inputMsg)
                .SetButtonPreset(MessageBoxButtonPreset.YesNo)
                .WithInputField("Join Code", 6, TMP_InputField.ContentType.Alphanumeric)
                .Build());

            if (inputResult.Button == MessageBoxButtonType.No) return;

            loadingOverlay.Show();
            var result = await TMMultiplayService.Instance.JoinRoomAsync(inputResult.InputValue, destroyCancellationToken);
            loadingOverlay.Hide();

            if (result.ErrorCode == ErrorCode.CANCELLED) return;

            if (!result.IsSuccess)
            {
                var errorMsg = LocalizedStrings.MainScene.Multiplay_JoinRoom_Failed.GetLocalizedString();
                await messageBox.Show(MessageBoxRequest.Create()
                    .SetMessage(errorMsg)
                    .SetButtonPreset(MessageBoxButtonPreset.Confirm)
                    .Build());
                return;
            }

            joinCodeTmp.text = string.Empty;
            EnterWaitingRoom();
        }

        // ── 방 나가기 ──────────────────────────────────────

        public void OnLeaveRoomButtonClicked() => OnLeaveRoomButtonClickedAsync().Forget();

        private async UniTaskVoid OnLeaveRoomButtonClickedAsync()
        {
            var confirmMsg = LocalizedStrings.MainScene.Multiplay_LeaveRoom_Message.GetLocalizedString();
            var confirmResult = await messageBox.Show(MessageBoxRequest.Create()
                .SetMessage(confirmMsg)
                .SetButtonPreset(MessageBoxButtonPreset.YesNo)
                .Build());

            if (confirmResult.Button == MessageBoxButtonType.No) return;

            ExitWaitingRoom();
            await TMMultiplayService.Instance.LeaveRoomAsync();
            waitingRoom.SetActive(false);
            multiplayMenu.SetActive(true);
        }

        // ── 대기방 진입 / 퇴장 ────────────────────────────

        private void EnterWaitingRoom()
        {
            leaveRoomButton.interactable = true;
            waitingRoom.SetActive(true);
            multiplayMenu.SetActive(false);

            TMMultiplayService.Instance.OnAllPlayersConnected += OnAllPlayersConnected;
        }

        private void ExitWaitingRoom()
        {
            TMMultiplayService.Instance.OnAllPlayersConnected -= OnAllPlayersConnected;
        }

        private void OnDestroy()
        {
            if (TMMultiplayService.Instance != null)
                TMMultiplayService.Instance.OnAllPlayersConnected -= OnAllPlayersConnected;
        }

        // ── 게임 시작 (호스트 전용) ────────────────────────

        private void OnAllPlayersConnected()
        {
            ExitWaitingRoom();
            leaveRoomButton.interactable = false;

            // 호스트만 씬 로드 명령. 클라이언트는 NGO가 자동으로 씬 이동.
            TMMultiplayService.Instance.StartGame();
        }
    }
}
