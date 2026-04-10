using System;
using TenMaker.Core.Input.Data;
using TenMaker.RankMode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TenMaker.Core.Input
{
    public class GamePlayInputHandler : InputHandler
    {
        public override string MapName => InputConstants.Maps.GAME_PLAY;

        public event Action<Vector2> DragStarted;
        public event Action<Vector2> DragPerformed;
        public event Action DragCanceled;

        protected override void AddCallbacks()
        {
            if (PlayerInput.ActionDict.TryGetValue(InputConstants.GamePlay.DRAG, out var dragAction) is false)
            {
                Debug.LogError($"{GetType().Name} - AddCallbacks: {InputConstants.GamePlay.DRAG} is not found.");
                return;
            }

            dragAction.started += OnDragStarted;
            dragAction.performed += OnDragPerformed;
            dragAction.canceled += OnDragCanceled;
        }

        protected override void RemoveCallbacks()
        {
            if (PlayerInput.ActionDict.TryGetValue(InputConstants.GamePlay.DRAG, out var dragAction) is false)
            {
                Debug.LogError($"{GetType().Name} - RemoveCallbacks: {InputConstants.GamePlay.DRAG} is not found.");
                return;
            }

            dragAction.started -= OnDragStarted;
            dragAction.performed -= OnDragPerformed;
            dragAction.canceled -= OnDragCanceled;
        }

        private void OnDragStarted(InputAction.CallbackContext context)
        {
            var pos = context.ReadValue<Vector2>();
            DragStarted?.Invoke(pos);
        }

        private void OnDragPerformed(InputAction.CallbackContext context)
        {
            var pos = context.ReadValue<Vector2>();
            DragPerformed?.Invoke(pos);
        }

        private void OnDragCanceled(InputAction.CallbackContext context)
        {
            DragCanceled?.Invoke();
        }
    }
}