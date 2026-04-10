using Sirenix.OdinInspector;
using TenMaker.Utility;
using UnityEditor;
using UnityEngine;

namespace TenMaker.Editor.Hub
{
    public class GeneralTab
    {
        private readonly TenMakerHubDataSO _hubData;

        public GeneralTab(TenMakerHubDataSO hubData)
        {
            _hubData = hubData;
            gameManagerPrefabPath = hubData.gameManagerPath;
        }

        [HorizontalGroup("Game Manager Prefab", Title = "Game Manager")]
        [Sirenix.OdinInspector.FilePath(Extensions = "prefab")]
        [OnValueChanged(nameof(OnGameManagerPrefabPathChanged))]
        public string gameManagerPrefabPath;

        [HorizontalGroup("Game Manager Prefab")]
        [Button("Open")]
        private void OpenGameManager()
        {
            if (string.IsNullOrEmpty(gameManagerPrefabPath))
            {
                TBMLog.HeaderError($"No Path Exists.");
                return;
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(gameManagerPrefabPath);
            if (prefab == null)
            {
                TBMLog.HeaderError($"No Prefab Exists.");
                return;
            }

            AssetDatabase.OpenAsset(prefab);
        }

        private void OnGameManagerPrefabPathChanged()
        {
            _hubData.gameManagerPath = $"Assets/" + gameManagerPrefabPath;
        }
    }
}