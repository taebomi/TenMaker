using TenMaker.Gameplay;
using TenMaker.Gameplay.CameraSystem;
using UnityEngine;

namespace TenMaker.Core
{
    [CreateAssetMenu(menuName = "TBM/Combo/Shake Data")]
    public class ComboShakeDataSO : ScriptableObject
    {
        [SerializeField] private CameraShakeData[] shakeData;

        public CameraShakeData GetData(int combo)
        {
            var idx = Mathf.Clamp(combo, 1, shakeData.Length);
            return shakeData[idx - 1];
        }
    }
}