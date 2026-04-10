using UnityEngine;

namespace TenMaker.Core
{
    public static class PlayData
    {
        public static bool IsFirstPlay;
        public static int BestScore;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            BestScore = 0;
            IsFirstPlay = false;
        }

    }
}