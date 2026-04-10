#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using TenMaker.Utility;
using TenMaker.Utils;
using UnityEditor;


namespace TenMaker.Editor.Hub
{
    public class TenMakerHub : OdinMenuEditorWindow
    {
        private TenMakerHubDataSO _hubDataSO;

        [MenuItem("Ten Maker/Hub")]
        private static void OpenWindow()
        {
            GetWindow<TenMakerHub>().Show();
        }


        protected override OdinMenuTree BuildMenuTree()
        {
            _hubDataSO = FindAndLoadHubData();
            if (_hubDataSO == null)
            {
                TBMLog.HeaderError($"{nameof(TenMakerHubDataSO)} is not found.");
                return null;
            }


            var tree = new OdinMenuTree(false)
            {
                { "General", new GeneralTab(_hubDataSO) },
                { "Settings", new SettingsTab() },
                { "Color Presets", new ColorPresetsTab(_hubDataSO) }
            };
            return tree;
        }

        /// <summary>
        /// 세이브 데이터 찾아서 불러옴.
        /// </summary>
        /// <returns></returns>
        private TenMakerHubDataSO FindAndLoadHubData()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(TenMakerHubDataSO)}");
            if (guids.Length == 0)
            {
                return null;
            }

            if (guids.Length > 1)
            {
                TBMLog.HeaderWarning($"several {nameof(TenMakerHubDataSO)} are found.");
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var hubData = AssetDatabase.LoadAssetAtPath<TenMakerHubDataSO>(path);
            if (hubData == null)
            {
                TBMLog.HeaderError($"Load Asset Failed.");
                return null;
            }

            return hubData;
        }
    }
}
#endif