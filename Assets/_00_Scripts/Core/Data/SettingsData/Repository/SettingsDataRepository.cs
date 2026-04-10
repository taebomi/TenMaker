using TenMaker.Core.Save;
using UnityEngine;

namespace TenMaker.Core.Data
{
    public class SettingsDataRepository : MonoBehaviour, ISettingsDataRepository
    {
        public GeneralSettingsData GeneralSettingsData { get; private set; }
        public AudioSettingsData AudioSettingsData { get; private set; }


        public void SetData(SettingsData data)
        {
            GeneralSettingsData = data.generalSettingsData;
            AudioSettingsData = data.audioSettingsData;
        }

        public SettingsData GetData()
        {
            var generalSettingsData = new GeneralSettingsData(GeneralSettingsData);
            var audioSettingsData = new AudioSettingsData(AudioSettingsData);
            return new SettingsData()
            {
                audioSettingsData = audioSettingsData,
                generalSettingsData = generalSettingsData,
            };
        }
    }
}