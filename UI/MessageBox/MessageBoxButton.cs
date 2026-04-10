using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenMaker.UI
{
    public class MessageBoxButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;
        
        private MessageBoxButtonType _type;

        public void Initialize(MessageBoxButtonType type, string label)
        {
            _type = type;
            text.text = label;
        }

        public void AddListener(Action<MessageBoxButtonType> onClick)
        {
            button.onClick.AddListener(() => onClick(_type));
        }

        public void RemoveAllListeners()
        {
            button.onClick.RemoveAllListeners();
        }
        
    }
}