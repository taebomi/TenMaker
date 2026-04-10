using UnityEngine;

namespace TenMaker.Gameplay.GridSystem
{
    [CreateAssetMenu(menuName = "TBM/Game Play/Grid Config")]
    public class GridConfigSO : ScriptableObject
    {
        public GridConfigData data;
    }
}