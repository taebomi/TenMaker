using FMODUnity;

namespace TenMaker.Core.Audio
{
    public interface IAudioManager
    {
        void PlayBgm(EventReference bgmRef);
        void StopBgm();
        void ResumeBgm();
        void SetBgmPitch(float pitch);
        void TransitionBgmPitch(float pitch);

        void PlaySfx(EventReference sfxRef);
        void PlaySfx(EventReference sfxRef, float pitch);

        void SetBgmMuted(bool muted);
        void SetBgmVolume(float value);
        void SetSfxMuted(bool muted);
        void SetSfxVolume(float value);
    }
}