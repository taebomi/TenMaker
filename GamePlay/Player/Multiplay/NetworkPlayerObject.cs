using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.Multiplay.Player;
using TenMaker.Gameplay.Player.Multiplay;
using TenMaker.Scenes.DuelMode;
using TenMaker.Utility;
using Unity.Netcode;
using UnityEngine;

namespace TenMaker.Gameplay.Multiplay
{
    public class NetworkPlayerObject : NetworkBehaviour
    {
        // modules
        [field: BoxGroup("Modules"), SerializeField]
        public NetworkPlayerCustomizationSystem CustomizationSystem { get; private set; }
        [field: BoxGroup("Modules"), SerializeField]
        public PlayerInputController InputController { get; private set; }

        // references
        public DuelModeController Controller { get; private set; }


        // properties
        public PlayerColor Color { get; private set; }

        public async UniTask LoadCustomizationAssets(PlayerCustomizationData data, CancellationToken ct)
        {
            Color = data.Color;
            await CustomizationSystem.InitializeAsync(data, ct);
        }

        public async UniTask InitializeAsync(DuelModeController controller, CancellationToken ct)
        {
            Controller = controller;
            await InitializeCore(ct);
            if (IsOwner) InitializeOwner();
        }

        private async UniTask InitializeCore(CancellationToken ct)
        {
            InputController.InitializeClient(Controller.RegionSelectionHandler);
            await UniTask.CompletedTask;
        }

        public void Deinitialize()
        {
            InputController.Deinitialize();
        }

        private void InitializeOwner()
        {
            TBMLog.HeaderLog(gameObject.name);
            InputController.InitializeOwner(Controller.MainCamera, Controller.GridController.GridView);
        }

        public void Enable()
        {
            InputController.Enable();
        }

        public void Disable()
        {
            InputController.Disable();
        }
    }
}