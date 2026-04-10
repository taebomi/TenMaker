using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Core.Scene
{
    public static class TMSceneService
    {
        public static ISceneService Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(ISceneService service)
        {
            if (Instance != null)
            {
                TBMLog.HeaderError($"Already initialized.");
                return;
            }

            Instance = service;
        }

        public static void Deinitialize(ISceneService service)
        {
            if (Instance != service)
            {
                TBMLog.HeaderError($"Not initialized.");
                return;
            }

            Instance = null;
        }
    }
}