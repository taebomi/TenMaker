using TenMaker.Core;
using TenMaker.Gameplay.CameraSystem;
using UnityEngine;

namespace TenMaker.Gameplay.Combo
{
    [CreateAssetMenu(fileName = "Combo Camera Shake Data SO", menuName = "Ten Maker/Combo/Camera Shake Data")]
    public class ComboCameraShakeDataSO : ScriptableObject
    {
        [SerializeField] private CameraShakeData[] dataArr;

        public CameraShakeData GetData(int combo)
        {
            var index = combo - GameRule.MINIMUM_COMBO;
            index = Mathf.Clamp(index, 0, dataArr.Length - 1);
            return dataArr[index];
        }
    }
}