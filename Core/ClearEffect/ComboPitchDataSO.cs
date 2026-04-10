using UnityEngine;

namespace TenMaker.Core.Player
{
    [CreateAssetMenu(fileName = "Combo Pitch Data", menuName = "TBM/Combo/Pitch Data", order = 0)]
    public class ComboPitchDataSO : ScriptableObject
    {
        [SerializeField] private float[] pitchValues;

        public float GetPitch(int combo)
        {
            var index = Mathf.Clamp(combo, 1, pitchValues.Length);
            return pitchValues[index - 1];
        }
    }
}