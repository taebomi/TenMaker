using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Core.Input
{
    public static class TMInputManager
    {
        public static IInputManager Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(IInputManager manager)
        {
            if (Instance != null)
            {
                TBMLog.HeaderError($"{typeof(TMInputManager)} is already initialized.");
                return;
            }

            Instance = manager;
        }

        public static void Deinitialize(IInputManager manager)
        {
            if (manager != Instance) return;

            Instance = null;
        }
    }
}