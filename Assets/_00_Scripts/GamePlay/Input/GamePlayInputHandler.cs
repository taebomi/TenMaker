using System;
using TenMaker.Core.Input;
using TenMaker.RankMode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TenMaker.Gameplay.Input
{
    public class GamePlayInputHandler : InputHandler
    {
        public override string MapName => InputConstants.Maps.GAME_PLAY;

        public event Action<Vector2> DragStarted;
        public event Action<Vector2> DragPerformed;
        public event Action DragCanceled;

        public GamePlayInputHandler(TMPlayerInput playerInput) : base(playerInput)
        {
        }

        protected override void AddInputCallbacks()
        {
            if (PlayerInput == null || PlayerInput.TryGetAction(InputConstants.GamePlay.DRAG, out var dragAction) is false)
                return;

            dragAction.started += OnDragStarted;
            dragAction.performed += OnDragPerformed;
            dragAction.canceled += OnDragCanceled;
        }

        protected override void RemoveInputCallbacks()
        {
            if (PlayerInput == null || PlayerInput.TryGetAction(InputConstants.GamePlay.DRAG, out var dragAction) is false)
                return;

            dragAction.started -= OnDragStarted;
            dragAction.performed -= OnDragPerformed;
            dragAction.canceled -= OnDragCanceled;
        }

        private void OnDragStarted(InputAction.CallbackContext context)
        {
            var point = context.ReadValue<Vector2>();
            DragStarted?.Invoke(point);
        }

        private void OnDragPerformed(InputAction.CallbackContext context)
        {
            var point = context.ReadValue<Vector2>();
            DragPerformed?.Invoke(point);
        }

        private void OnDragCanceled(InputAction.CallbackContext context)
        {
            DragCanceled?.Invoke();
        }
    }
}