using System;

namespace TenMaker.Utility
{
    public static partial class TBMUtility
    {
        private const float OvershootOrAmplitude = 1.70158f;
    
        public static float OutQuad(float time, float duration)
        {
            return -(time /= duration) * (time - 2f);
        }

        public static float InSine(float time, float duration)
        {
            return (float)(-Math.Cos((double)time / duration * 1.5707963705062866) + 1.0);
        }

        public static float OutSine(float time, float duration)
        {
            return (float)Math.Sin((double)time / duration * 1.5707963705062866);
        }

        public static float InOutSine(float time, float duration)
        {
            return (float)(-0.5 * (Math.Cos((double)time / duration * 3.1415927410125732) - 1.0));
        }

        public static float InQuad(float time, float duration)
        {
            return (time /= duration) * time * time;
        }
    
        public static float InCubic(float time, float duration)
        {
            return (time /= duration) * time * time;
        }

        public static float InBack(float time, float duration)
        {
            return (time /= duration) * time * ((OvershootOrAmplitude + 1.0f) * time - OvershootOrAmplitude);
        }

        public static float OutBack(float time, float duration)
        {
            return (time = ( time / duration - 1.0f)) * time * ((OvershootOrAmplitude + 1.0f) * time + OvershootOrAmplitude) + 1.0f;
        }
    }
}