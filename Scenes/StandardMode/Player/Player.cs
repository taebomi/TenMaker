using TenMaker.Core;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public InputSystem InputSystem { get; private set; }
        [field: SerializeField] public ClearHandler ClearHandler { get; private set; }
        [field: SerializeField] public ComboSystem ComboSystem { get; private set; }
        [field: SerializeField] public CameraSystem CameraSystem { get; private set; }

        private void OnDisable()
        {
            RemoveCallbacks();
        }

        public void Setup(Camera cam, GridManager gridManager)
        {
            InputSystem.Setup(cam, gridManager);
            ClearHandler.Setup();

            AddCallbacks();
        }

        private void AddCallbacks()
        {
            InputSystem.InputProcessor.DragCanceled += ClearHandler.RequestAreaClear;
        }

        private void RemoveCallbacks()
        {
            InputSystem.InputProcessor.DragCanceled -= ClearHandler.RequestAreaClear;
        }

        public void SetInput(bool value)
        {
            if (value)
            {
                InputSystem.Enable();
            }
            else
            {
                InputSystem.Disable();
            }
        }

        public void HandleGameOver() { }

        public void HandleAreaCleared()
        {
            ComboSystem.AddCombo();
            ClearHandler.HandleClear(ComboSystem.Combo);
            CameraSystem.ShakeCamera(ComboSystem.Combo);
        }
    }
}