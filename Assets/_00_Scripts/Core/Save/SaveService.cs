using UnityEngine;

namespace TenMaker.Core.Save
{
    public static class SaveService
    {
        public static ISaveManager Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(ISaveManager manager)
        {
            if (Instance != null)
            {
                Debug.LogError("SaveService is already initialized.");
                return;
            }

            Instance = manager;
        }

        public static void Deinitialize(ISaveManager manager)
        {
            if (manager != Instance) return;

            Instance = null;
        }
    }
}