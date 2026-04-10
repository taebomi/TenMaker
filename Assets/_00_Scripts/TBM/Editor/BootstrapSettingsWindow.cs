#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using TenMaker.Utility;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace TenMaker.Utils
{
    public class BootstrapSettingsWindow : OdinEditorWindow
    {
        [MenuItem("TBM/Bootstrap Settings")]
        static void Open()
        {
            GetWindow<BootstrapSettingsWindow>().Show();
        }

        [OnValueChanged(nameof(CheckBootstrap))]public SceneAsset bootstrapScene;
        [ToggleLeft, OnValueChanged(nameof(CheckBootstrap))]
        public bool isEnabled;

        private void CheckBootstrap()
        {
            if (bootstrapScene == null && isEnabled)
            {
                Debug.LogWarning($"Bootstrap scene is not set. Disabling the setting.");
                isEnabled = false;
                return;
            }
            
            EditorSceneManager.playModeStartScene = isEnabled ? bootstrapScene : null;
        }
    }
}

#endif