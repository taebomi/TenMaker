using TenMaker.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class CameraSystem : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCam { get; private set; }
        [SerializeField] private CinemachineCamera cam;

        [SerializeField] private CameraShaker cameraShaker;

        [SerializeField] private ComboShakeDataSO shakeDataSO;

        public void Setup()
        {
            AdjustCameraSize();

        }


        public void ShakeCamera(int combo)
        {
            if (combo <= 1) return;

            var shakeData = shakeDataSO.GetData(combo);
            cameraShaker.Shake(shakeData);
        }

        private void AdjustCameraSize()
        {
            // todo 화면 비율이 16:9보다 가로로 짧을 경우 카메라 사이즈 조절
        }

        private void SetCameraSize(float size)
        {
            cam.Lens.OrthographicSize = size;
        }
    }
}