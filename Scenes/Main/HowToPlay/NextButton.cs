using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

namespace TenMaker.MainScene.HowToPlay
{
    public class NextButton : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent localizeStringEvent;
        [SerializeField] private LocalizedString[] textKeys;

        private void OnEnable() { }

        public void UpdateText(int step)
        {
            localizeStringEvent.StringReference = textKeys[step];
        }
    }
}