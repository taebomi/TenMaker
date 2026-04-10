using TMPro;

namespace TenMaker.UI
{
    public class MessageBoxRequest
    {
        public string Message;
        public MessageBoxButtonPreset ButtonPreset;
        public InputFieldConfig? InputField;

        private MessageBoxRequest() { }

        public static Builder Create() => new();

        public class Builder
        {
            private readonly MessageBoxRequest _request;

            public Builder()
            {
                _request = new MessageBoxRequest();
            }

            public Builder SetMessage(string message)
            {
                _request.Message = message;
                return this;
            }

            public Builder SetButtonPreset(MessageBoxButtonPreset buttonPreset)
            {
                _request.ButtonPreset = buttonPreset;
                return this;
            }

            public Builder WithInputField(string placeHolder, int maxLength, TMP_InputField.ContentType contentType)
            {
                _request.InputField = new InputFieldConfig
                {
                    PlaceHolder = placeHolder,
                    MaxLength = maxLength,
                    ContentType = contentType
                };
                return this;
            }

            public MessageBoxRequest Build() => _request;
        }
    }

    public struct InputFieldConfig
    {
        public string PlaceHolder;
        public int MaxLength;
        public TMP_InputField.ContentType ContentType;
    }
}