using System;
using TenMaker.Services.UGS.Multiplay;
using UnityEngine;

namespace TenMaker.Services.UGS.Multiplay
{
    public static class TMMultiplayService
    {
        public static ITMMultiplayService Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(ITMMultiplayService instance)
        {
            if (Instance != null) throw new Exception("Already initialized.");

            Instance = instance;
        }

        public static void Deinitialize(ITMMultiplayService instance)
        {
            if (Instance == null) throw new Exception($"No instance exists.");
            if (instance != Instance) throw new Exception($"Instance mismatch.");

            Instance = null;
        }
    }
}