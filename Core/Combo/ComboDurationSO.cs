using UnityEngine;

namespace TenMaker.Core
{
    [CreateAssetMenu(fileName = "Combo Duration", menuName = "TBM/Combo/Duration", order = 0)]
    public class ComboDurationSO : ScriptableObject
    {
        [SerializeField] private float[] comboDurations;
        public int Length => comboDurations.Length;

        public float this[int index] => comboDurations[index];
    }
}