using TenMaker.Utility;
using UnityEditor;
using UnityEngine;

namespace TenMaker.Editor
{
    public static class CustomObjectCreator
    {
        [MenuItem("GameObject/Ten Maker/UI/Dimmed Background", false, 0)]
        public static void CreateDimmedBackground(MenuCommand menuCommand)
        {
            const string prefabPath = "Assets/_02_Prefabs/UI/Dimmed Background.prefab";
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null)
            {
                TBMLog.HeaderError($"{prefabPath} not exists.");
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(instance, "Create Dimmed Background");
            Selection.activeObject = instance;
        }
    }
}