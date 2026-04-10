using System;

namespace TenMaker.Gameplay.CameraSystem
{
    [Serializable]
    public struct CameraShakeData
    {
        public float intensity;
        public float duration;

        public CameraShakeData(float intensity, float duration)
        {
            this.intensity = intensity;
            this.duration = duration;
        }

        public bool IsValid()
        {
            return intensity > 0 && duration > 0;
        }

        public override string ToString()
        {
            return $"Intensity: {intensity}, Duration: {duration}";
        }
    }
}