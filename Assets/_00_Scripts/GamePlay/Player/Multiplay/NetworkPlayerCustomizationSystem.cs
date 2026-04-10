using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Utility;
using TenMaker.Gameplay;
using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.RegionClear;
using UnityEngine;

namespace TenMaker.Gameplay.Multiplay.Player
{
    public class NetworkPlayerCustomizationSystem : MonoBehaviour, IClearEffectProvider, ISelectionVisualizerProvider,
        IPlayerCustomizationProvider
    {
        [field: SerializeField] public PlayerSelectionVisualizer PlayerSelectionVisualizer { get; private set; }
        [field: SerializeField] public ClearEffector ClearEffector { get; private set; }


        // Default Customization Prefabs, 실패 시 기본값 사용
        [SerializeField] private CheckeredCellBackgroundProvider defaultCellBackgroundProvider;
        [SerializeField] private SpriteCellNumber defaultCellNumberPrefab;

        // Local Customization Instances


        // Network Customization Instances


        public async UniTask InitializeAsync(PlayerCustomizationData data, CancellationToken ct)
        {
            // todo addressable 사용하여 로드하는 것으로 수정할 것, 현재는 기본값 사용
            ClearEffector.Initialize();
            await UniTask.CompletedTask;
        }

        public ClearEffector GetClearEffector()
        {
            return ClearEffector;
        }

        public PlayerSelectionVisualizer GetSelectionVisualizer()
        {
            return PlayerSelectionVisualizer;
        }

        public CellNumber GetCellNumber()
        {
            return defaultCellNumberPrefab;
        }

        public ICustomCellBackgroundProvider GetCellBackgroundProvider()
        {
            return defaultCellBackgroundProvider;
        }
    }
}