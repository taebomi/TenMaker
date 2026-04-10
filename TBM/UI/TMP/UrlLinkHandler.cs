using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TenMaker.UI.TMP
{
    [RequireComponent(typeof(TMP_Text))]
    public class UrlLinkHandler : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField] private string url;


        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(url);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Application.OpenURL(url);
        }
    }
}