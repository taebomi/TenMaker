using TenMaker.Gameplay.Input;
using TenMaker.Core.Input;
using TenMaker.Gameplay.Player;
using Unity.Netcode;
using UnityEngine;
using GamePlayInputHandler = TenMaker.Gameplay.Input.GamePlayInputHandler;

namespace TenMaker.Gameplay.Multiplay.Player
{
    public class PlayerInputController : NetworkBehaviour
    {
        // Modules
        public GamePlayInputHandler InputHandler { get; private set; }
        public PlayerInputProcessor InputProcessor { get; private set; }

        // References
        private RegionSelectionHandler _selectionHandler;


        /// <summary>
        /// 공통 초기화 
        /// </summary>
        public void InitializeClient(RegionSelectionHandler regionSelectionHandler)
        {
            _selectionHandler = regionSelectionHandler;
        }

        /// <summary>
        /// Owner 초기화
        /// </summary>
        public void InitializeOwner(Camera cam, GridView gridView)
        {
            InputProcessor = new PlayerInputProcessor(cam, gridView.GetCellObject, gridView.GetNearestCellObject);
            InputHandler = new GamePlayInputHandler(TMInputManager.Instance.Player);
            InputHandler.Initialize();
            AddCallbacks();
        }

        public void Deinitialize()
        {
            RemoveCallbacks();
            InputHandler?.Deinitialize();
            InputHandler = null;
            InputProcessor = null;
        }

        public void Enable()
        {
            InputHandler.Enable();
        }

        public void Disable()
        {
            InputHandler?.Disable();
        }

        private void AddCallbacks()
        {
            if (InputHandler != null)
            {
                InputHandler.DragStarted += ProcessDragStarted;
                InputHandler.DragPerformed += ProcessDragPerformed;
                InputHandler.DragCanceled += ProcessDragCanceled;
            }

            if (InputProcessor != null)
            {
                InputProcessor.DragPerformed += HandleCellDragPerformed;
                InputProcessor.DragCanceled += HandleCellDragCanceled;
            }
        }

        private void RemoveCallbacks()
        {
            if (InputHandler != null)
            {
                InputHandler.DragStarted -= ProcessDragStarted;
                InputHandler.DragPerformed -= ProcessDragPerformed;
                InputHandler.DragCanceled -= ProcessDragCanceled;
            }

            if (InputProcessor != null)
            {
                InputProcessor.DragPerformed -= HandleCellDragPerformed;
                InputProcessor.DragCanceled -= HandleCellDragCanceled;
            }
        }

        // Processor
        private void ProcessDragStarted(Vector2 point)
        {
            InputProcessor.OnDragStarted(point);
        }

        private void ProcessDragPerformed(Vector2 point)
        {
            InputProcessor.OnDragPerformed(point);
        }

        private void ProcessDragCanceled()
        {
            InputProcessor.OnDragCanceled();
        }

        // Handler
        /// <summary>
        /// 드래그 진행 중 처리
        /// </summary>
        private void HandleCellDragPerformed(GridSelectionData data)
        {
            var selectedRegion = data.ToRegion();
            _selectionHandler.PerformLocalDrag(selectedRegion);
            PerformDragServerRpc(selectedRegion);
        }

        /// <summary>
        /// 드래그 종료 처리
        /// </summary>
        private void HandleCellDragCanceled(GridSelectionData data)
        {
            var selectedRegion = data.ToRegion();
            _selectionHandler.FinishLocalDrag();
            FinishDragServerRpc(selectedRegion);
        }

        // Server Rpc
        [Rpc(SendTo.Server, RequireOwnership = true)]
        private void PerformDragServerRpc(Region selectedRegion, RpcParams rpcParams = default)
        {
            var senderId = rpcParams.Receive.SenderClientId;
            _selectionHandler.UpdateSelectionServer(senderId, selectedRegion);
        }

        [Rpc(SendTo.Server, RequireOwnership = true)]
        private void FinishDragServerRpc(Region selectedRegion, RpcParams rpcParams = default)
        {
            var senderId = rpcParams.Receive.SenderClientId;
            _selectionHandler.FinishSelectionServer(senderId, selectedRegion);
        }
    }
}