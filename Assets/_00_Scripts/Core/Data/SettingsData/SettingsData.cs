using System;

namespace TenMaker.Core.Save
{
    [Serializable]
    public class SettingsData
    {
        public GeneralSettingsData generalSettingsData;
        public AudioSettingsData audioSettingsData;
    }
}