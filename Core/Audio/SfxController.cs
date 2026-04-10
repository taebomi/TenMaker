using FMODUnity;
using UnityEngine;

namespace TenMaker.Core.Audio
{
    public class SfxController : MonoBehaviour
    {
        public AudioChannelConfig Config => _config;
        private AudioChannelConfig _config;

        private void Awake()
        {
            _config = AudioChannelConfig.Default;
        }

        public void SetMuted(bool muted)
        {
            _config.muted = muted;
        }

        public void SetVolume(float volume)
        {
            _config.volume = volume;
        }

        public void SetConfig(AudioChannelConfig config)
        {
            _config = config;
        }

        public void Play(EventReference sfx)
        {
            if (_config.muted) return;

            var instance = RuntimeManager.CreateInstance(sfx);
            instance.setVolume(_config.volume);
            instance.start();
            instance.release();
        }

        public void Play(EventReference sfx, float pitch)
        {
            if (_config.muted) return;

            var instance = RuntimeManager.CreateInstance(sfx);
            instance.setVolume(_config.volume);
            instance.setPitch(pitch);
            instance.start();
            instance.release();
        }
    }
}