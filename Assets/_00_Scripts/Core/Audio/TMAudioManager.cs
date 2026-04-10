using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Core.Audio
{
    public static class TMAudioManager
    {
        public static IAudioManager Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(IAudioManager manager)
        {
            if (Instance != null)
            {
                TBMLog.HeaderError($"{typeof(TMAudioManager)} is already initialized.");
                return;
            }

            Instance = manager;
        }

        public static void Deinitialize(IAudioManager manager)
        {
            if (manager != Instance) return;

            Instance = null;
        }
    }
}