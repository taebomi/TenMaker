using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Core
{
    public static class TMGameManager
    {
        public static IGameManager Instance { get; private set; }
        
        public static bool IsInstanceExist => !ReferenceEquals(Instance, null);
        public static bool IsInstance(IGameManager manager) => ReferenceEquals(Instance, manager);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            Instance = null;
            
            var gameManagerPrefab = Resources.Load<GameObject>("Core/Game Manager");
            if (gameManagerPrefab == null)
            {
                TBMLog.HeaderError($"Game Manager is null");
            }
            else
            {
                Object.Instantiate(gameManagerPrefab);
            }
        }

        public static void Initialize(IGameManager manager)
        {
            if (Instance != null)
            {
                TBMLog.HeaderError("Already Initialized");
                return;
            }

            Instance = manager;
        }

        public static void Deinitialize(IGameManager manager)
        {
            if (Instance == null || Instance != manager)
            {
                TBMLog.HeaderError("Not Initialized");
                return;
            }

            Instance = null;
        }
    }
}