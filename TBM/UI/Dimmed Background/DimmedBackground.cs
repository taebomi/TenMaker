using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace TenMaker.UI
{
    public class DimmedBackground : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float TAP_TIME_THRESHOLD = 0.2f;
        private const float TAP_DISTANCE_THRESHOLD = 50f;

        [SerializeField] private UnityEvent tapEvent;

        private RectTransform _rt;
        private Canvas _canvas;

        private static float _pointerDownTime;
        private static Vector2 _pointerDownPosition;

        private void Awake()
        {
            _rt = (RectTransform)transform;
            _canvas = GetComponentInParent<Canvas>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDownTime = Time.time;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rt, eventData.position, _canvas.worldCamera,
                out _pointerDownPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var duration = Time.time - _pointerDownTime;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rt, eventData.position, _canvas.worldCamera,
                out var pointerUpPosition);
            var distance = Vector2.Distance(_pointerDownPosition, pointerUpPosition);
            if (duration > TAP_TIME_THRESHOLD || distance > TAP_DISTANCE_THRESHOLD) return;

            tapEvent?.Invoke();
        }
    }
}