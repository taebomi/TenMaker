using FMODUnity;
using TenMaker.Core.Data;
using TenMaker.Core.Save;
using UnityEngine;

namespace TenMaker.Core.Audio
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField] private BgmController bgmController;
        [SerializeField] private SfxController sfxController;

        public void Initialize()
        {
            TMAudioManager.Initialize(this);
        }

        public void Deinitialize()
        {
            TMAudioManager.Deinitialize(this);
        }

        public void ApplySaveData()
        {
            var audioSettings = TMSettingsDataRepository.Instance.AudioSettingsData;
            ApplyAudioSettings(audioSettings);
        }

        #region Bgm

        public void PlayBgm(EventReference bgmRef)
        {
            bgmController.Play(bgmRef);
        }

        public void StopBgm()
        {
            bgmController.Stop();
        }

        public void ResumeBgm()
        {
            bgmController.Resume();
        }

        public void SetBgmPitch(float pitch)
        {
            bgmController.SetPitchInstantly(pitch);
        }

        public void TransitionBgmPitch(float pitch)
        {
            bgmController.SetPitchSmoothly(pitch);
        }

        #endregion

        #region Sfx

        public void PlaySfx(EventReference sfxRef)
        {
            sfxController.Play(sfxRef);
        }

        public void PlaySfx(EventReference sfxRef, float pitch)
        {
            sfxController.Play(sfxRef, pitch);
        }

        #endregion

        #region Config

        public void ApplyAudioSettings(AudioSettingsData data)
        {
            bgmController.SetConfig(data.bgmConfig);
            sfxController.SetConfig(data.sfxConfig);
        }

        public void SetBgmMuted(bool muted)
        {
            bgmController.SetMuted(muted);
        }

        public void SetBgmVolume(float value)
        {
            bgmController.SetVolume(value);
        }

        public void SetSfxMuted(bool muted)
        {
            sfxController.SetMuted(muted);
        }

        public void SetSfxVolume(float value)
        {
            sfxController.SetVolume(value);
        }

        #endregion
    }
}