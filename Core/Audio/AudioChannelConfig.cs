using System;
using UnityEngine;

namespace TenMaker.Core.Audio
{
    [Serializable]
    public struct AudioChannelConfig
    {
        public bool muted;
        public float volume;

        public static AudioChannelConfig Default => new(false, 1f);

        public AudioChannelConfig(bool muted, float volume)
        {
            this.muted = muted;
            this.volume = volume;
        }

        public AudioChannelConfig(AudioChannelConfig config)
        {
            muted = config.muted;
            volume = config.volume;
        }

        public static AudioChannelConfig Combine(AudioChannelConfig a, AudioChannelConfig b)
        {
            return new AudioChannelConfig(a.muted || b.muted, a.volume * b.volume);
        }

        public bool IsEqual(AudioChannelConfig other)
        {
            return muted == other.muted && Mathf.Approximately(volume, other.volume);
        }
    }
}