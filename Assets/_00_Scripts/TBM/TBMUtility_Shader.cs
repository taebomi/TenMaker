using UnityEngine;

namespace TenMaker.Utility
{
    public static class TBMUtility_Shader
    {
        public static int MainTex;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            MainTex = Shader.PropertyToID("_MainTex");
        }
    }
}