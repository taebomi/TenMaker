using TenMaker.Core.Data;
using UnityEngine;

namespace TenMaker.Core.Save
{
    public class SaveManager : MonoBehaviour, ISaveManager
    {
        private SettingsDataStorage _settingsDataStorage;
        private UserDataStorage _userDataStorage;

        [field:SerializeField] public UserDataRepository UserDataRepository { get; set; }
        [field:SerializeField] public SettingsDataRepository SettingsDataRepository { get; set; }


        public void Initialize()
        {
            SaveService.Initialize(this);

            _userDataStorage = new UserDataStorage();
            _settingsDataStorage = new SettingsDataStorage();

            TMUserDataRepository.Initialize(UserDataRepository);
            TMSettingsDataRepository.Initialize(SettingsDataRepository);
        }

        public void Deinitialize()
        {
            TMUserDataRepository.Deinitialize(UserDataRepository);
            TMSettingsDataRepository.Deinitialize(SettingsDataRepository);

            SaveService.Deinitialize(null);
        }

        public void Load()
        {
            var settingsData = _settingsDataStorage.Load();
            var userData = _userDataStorage.Load();

            SettingsDataRepository.SetData(settingsData);
            UserDataRepository.SetData(userData);
        }

        public void Save()
        {
            var settingsData = SettingsDataRepository.GetData();
            _settingsDataStorage.Save(settingsData);
            
            var userData = UserDataRepository.GetData();
            _userDataStorage.Save(userData);
            PlayerPrefs.Save();
        }
    }
}