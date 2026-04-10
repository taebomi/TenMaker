using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace TenMaker.Core.Audio
{
    public class BgmController : MonoBehaviour
    {
        private const float PITCH_CHANGING_SPEED = 0.5f;

        public AudioChannelConfig Config => _config;
        private AudioChannelConfig _config;

        private EventInstance _curBgmInstance;

        private float _curPitch;
        private float _destPitch;

        private bool _isPitchTransitioning;

        private void Awake()
        {
            _config = AudioChannelConfig.Default;
            _curPitch = 1f;
            _destPitch = 1f;
            _isPitchTransitioning = false;
        }


        #region Config

        public void SetMuted(bool muted)
        {
            _config.muted = muted;
            ApplyConfig();
        }

        public void SetVolume(float volume)
        {
            _config.volume = volume;
            ApplyConfig();
        }

        public void SetConfig(AudioChannelConfig config)
        {
            _config = config;
            ApplyConfig();
        }

        private void ApplyConfig()
        {
            var volume = _config.muted ? 0f : _config.volume;
            if (_curBgmInstance.isValid())
            {
                _curBgmInstance.setVolume(volume);
            }
        }

        #endregion

        public void Play(EventReference bgmRef)
        {
            Stop();

            _curBgmInstance = RuntimeManager.CreateInstance(bgmRef);
            _curBgmInstance.setVolume(_config.volume);
            _curBgmInstance.setPitch(_curPitch);
            _curBgmInstance.start();
        }

        public void Resume()
        {
            if (_curBgmInstance.isValid())
            {
                _curBgmInstance.setVolume(_config.volume);
                _curBgmInstance.setPitch(_curPitch);
                _curBgmInstance.start();
            }
        }

        public void Stop()
        {
            if (_curBgmInstance.isValid())
            {
                _curBgmInstance.setCallback(null);
                _curBgmInstance.stop(STOP_MODE.ALLOWFADEOUT);
                _curBgmInstance.release();
            }
        }

        public void SetPitchInstantly(float pitch)
        {
            _curPitch = pitch;

            if (_curBgmInstance.isValid())
            {
                _curBgmInstance.setPitch(_curPitch);
            }
        }

        public void SetPitchSmoothly(float pitch)
        {
            _destPitch = pitch;

            if (_isPitchTransitioning) return;
            TransitionPitchAsync().Forget();
        }

        private async UniTaskVoid TransitionPitchAsync()
        {
            _isPitchTransitioning = true;

            while (Mathf.Approximately(_curPitch, _destPitch))
            {
                var pitch = Mathf.MoveTowards(_curPitch, _destPitch, PITCH_CHANGING_SPEED * Time.unscaledTime);
                SetPitchInstantly(pitch);
                await UniTask.Yield(destroyCancellationToken);
            }

            _isPitchTransitioning = false;
        }
    }
}