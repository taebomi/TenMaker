using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Core.Data
{
    public static class TMUserDataRepository
    {
        public static IUserDataRepository Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(IUserDataRepository repository)
        {
            if (Instance != null)
            {
                TBMLog.HeaderError($"{typeof(TMUserDataRepository)} is already initialized.");
                return;
            }

            Instance = repository;
        }

        public static void Deinitialize(IUserDataRepository repository)
        {
            if (repository != Instance) return;

            Instance = null;
        }
    }
}