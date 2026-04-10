using Unity.Cinemachine;
using UnityEngine;

namespace TenMaker.Gameplay.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        [field: SerializeField] public Camera Cam { get; private set; }
        [SerializeField] private CinemachineCamera cinemachineCam;
        [SerializeField] private CinemachineBasicMultiChannelPerlin perlin;

        private CameraShaker _cameraShaker;

        private void Awake()
        {
            _cameraShaker = new CameraShaker(perlin);
        }

        public void ShakeCamera(CameraShakeData shakeData)
        {
            if (shakeData.IsValid() is false) return;

            _cameraShaker?.Shake(shakeData, destroyCancellationToken);
        }
    }
}