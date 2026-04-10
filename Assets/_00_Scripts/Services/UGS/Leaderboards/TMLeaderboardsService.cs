using System;
using UnityEngine;

namespace TenMaker.Services.UGS.Leaderboards
{
    public static class TMLeaderboardsService
    {
        public static ITMLeaderboardsService Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(ITMLeaderboardsService instance)
        {
            if (Instance != null) throw new Exception("Already initialized.");

            Instance = instance;
        }

        public static void Deinitialize(ITMLeaderboardsService instance)
        {
            if (Instance == null) throw new Exception($"No instance exists.");
            if (instance != Instance) throw new Exception($"Instance mismatch.");

            Instance = null;
        }
    }
}