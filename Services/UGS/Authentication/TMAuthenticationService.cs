using System;
using UnityEngine;

namespace TenMaker.Services.UGS.Authentication
{
    public static class TMAuthenticationService
    {
        public static ITMAuthenticationService Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(ITMAuthenticationService instance)
        {
            if (Instance != null) throw new Exception("Already initialized.");

            Instance = instance;
        }

        public static void Deinitialize(ITMAuthenticationService instance)
        {
            if (Instance == null) throw new Exception($"No instance exists.");
            if (instance != Instance) throw new Exception($"Instance mismatch.");

            Instance = null;
        }
    }
}