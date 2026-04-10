using TenMaker.Utility;
using TenMaker.Core;
using TenMaker.Core.Input;
using TenMaker.Gameplay.Input;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class InputSystem : MonoBehaviour
    {
        [field: SerializeField] public InputProcessor InputProcessor { get; private set; }
        [SerializeField] private GamePlayInputHandler inputHandler;

        [SerializeField] private SelectionVisualizer visualizer;

        private Camera _mainCam;

        private void OnEnable()
        {
            inputHandler.DragStarted += OnDragStarted;
            inputHandler.DragPerformed += OnDragPerformed;
            inputHandler.DragCanceled += OnDragCanceled;

            InputProcessor.DragStarted += visualizer.Show;
            InputProcessor.DragPerformed += visualizer.UpdateSelection;
        }

        private void OnDisable()
        {
            inputHandler.DragStarted -= OnDragStarted;
            inputHandler.DragPerformed -= OnDragPerformed;
            inputHandler.DragCanceled -= OnDragCanceled;

            InputProcessor.DragStarted -= visualizer.Show;
            InputProcessor.DragPerformed -= visualizer.UpdateSelection;

            inputHandler.Disable();
        }

        public void Setup(Camera cam, GridManager gridManager)
        {
            _mainCam = cam;
            InputProcessor.Setup(gridManager.GetCell, gridManager.GetNearestCell);
            inputHandler.Initialize();
        }

        public void Enable()
        {
            inputHandler.Enable();
        }

        public void Disable()
        {
            inputHandler.Disable();
        }

        private void OnDragStarted(Vector2 pos)
        {
            var worldPos = _mainCam.ScreenToWorldPoint(pos);
            InputProcessor.OnDragStarted(worldPos);
        }

        private void OnDragPerformed(Vector2 pos)
        {
            var worldPos = _mainCam.ScreenToWorldPoint(pos);
            InputProcessor.OnDragPerformed(worldPos);
        }

        private void OnDragCanceled()
        {
            InputProcessor.OnDragCanceled();
            visualizer.Hide();
        }
    }
}