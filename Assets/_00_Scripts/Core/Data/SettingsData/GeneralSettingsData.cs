using System;

namespace TenMaker.Core.Save
{
    [Serializable]
    public class GeneralSettingsData
    {
        public string localeCode;

        public GeneralSettingsData(string localeCode)
        {
            this.localeCode = localeCode;
        }

        public GeneralSettingsData(GeneralSettingsData ori)
        {
            localeCode = ori.localeCode;
            
        }
    }
}