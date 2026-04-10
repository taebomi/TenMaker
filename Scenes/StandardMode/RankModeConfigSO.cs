using UnityEngine;

namespace TenMaker.RankMode
{
    [CreateAssetMenu(fileName = "RankModeConfig", menuName = "TBM/RankMode/Config", order = 0)]
    public class RankModeConfigSO : ScriptableObject
    {
        public int col = 10;
        public int row = 17;
        public float time = 120f;
    }
}