using System;
using UnityEngine;

namespace TenMaker.Core.Localization
{
    public static class TMLocalizationManager
    {
        public static ITMLocalizationManager Instance { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(ITMLocalizationManager manager)
        {
            if (Instance != null) throw new Exception($"Already Initialized");
            Instance = manager;
        }

        public static void Deinitialize(ITMLocalizationManager manager)
        {
            if (Instance != manager) throw new Exception($"Not Initialized");
            Instance = null;
        }
    }
}