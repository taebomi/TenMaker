using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Core.Data
{
    public class TMSettingsDataRepository
    {
        public static ISettingsDataRepository Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void OnSubsystemRegistration()
        {
            Instance = null;
        }

        public static void Initialize(ISettingsDataRepository repository)
        {
            if (Instance != null)
            {
                TBMLog.HeaderError($"{typeof(TMSettingsDataRepository)} is already initialized.");
                return;
            }

            Instance = repository;
        }

        public static void Deinitialize(ISettingsDataRepository repository)
        {
            if (repository != Instance) return;

            Instance = null;
        }
    }
}