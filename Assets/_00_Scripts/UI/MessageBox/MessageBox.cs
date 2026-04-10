using System;
using Cysharp.Threading.Tasks;
using TenMaker.Core.Localization;
using TMPro;
using UnityEngine;

namespace TenMaker.UI
{
    public class MessageBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text message;

        [SerializeField] private TMP_InputField inputfield;
        [SerializeField] private TMP_Text inputfieldPlaceHolder;

        [SerializeField] private MessageBoxButton confirmButton;
        [SerializeField] private MessageBoxButton yesButton;
        [SerializeField] private MessageBoxButton noButton;

        private UniTaskCompletionSource<MessageBoxResult> _resultTcs;

        private void Awake()
        {
            confirmButton.Initialize(MessageBoxButtonType.Confirm,
                LocalizedStrings.System.MessageBoxButton_Confirm.GetLocalizedString());
            yesButton.Initialize(MessageBoxButtonType.Yes,
                LocalizedStrings.System.MessageBoxButton_Yes.GetLocalizedString());
            noButton.Initialize(MessageBoxButtonType.No,
                LocalizedStrings.System.MessageBoxButton_No.GetLocalizedString());
        }


        public UniTask<MessageBoxResult> Show(MessageBoxRequest request)
        {
            message.text = request.Message;


            var isYesOrNo = request.ButtonPreset == MessageBoxButtonPreset.YesNo;
            confirmButton.gameObject.SetActive(!isYesOrNo);
            yesButton.gameObject.SetActive(isYesOrNo);
            noButton.gameObject.SetActive(isYesOrNo);

            if (request.InputField.HasValue)
            {
                inputfieldPlaceHolder.text = request.InputField.Value.PlaceHolder;
                inputfield.characterLimit = request.InputField.Value.MaxLength;
                inputfield.contentType = request.InputField.Value.ContentType;

                inputfield.gameObject.SetActive(true);
            }
            else
            {
                inputfield.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);

            confirmButton.RemoveAllListeners();
            yesButton.RemoveAllListeners();
            noButton.RemoveAllListeners();
            
            _resultTcs = new UniTaskCompletionSource<MessageBoxResult>();

            confirmButton.AddListener(OnButtonClicked);
            yesButton.AddListener(OnButtonClicked);
            noButton.AddListener(OnButtonClicked);
            return _resultTcs.Task;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnButtonClicked(MessageBoxButtonType type)
        {
            _resultTcs?.TrySetResult(new MessageBoxResult()
            {
                Button = type,
                InputValue = inputfield.text,
            });
            _resultTcs = null;
            Hide();
        }
    }
}