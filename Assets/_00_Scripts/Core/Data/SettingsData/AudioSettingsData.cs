using System;
using TenMaker.Core.Audio;
using TenMaker.Core.Data;

namespace TenMaker.Core.Save
{
    [Serializable]
    public class AudioSettingsData
    {
        public AudioChannelConfig bgmConfig;
        public AudioChannelConfig sfxConfig;

        public AudioSettingsData(AudioChannelConfig bgmConfig, AudioChannelConfig sfxConfig)
        {
            this.bgmConfig = bgmConfig;
            this.sfxConfig = sfxConfig;
        }

        public AudioSettingsData(AudioSettingsData ori)
        {
            bgmConfig = new AudioChannelConfig(ori.bgmConfig);
            sfxConfig = new AudioChannelConfig(ori.sfxConfig);
        }
    }
}