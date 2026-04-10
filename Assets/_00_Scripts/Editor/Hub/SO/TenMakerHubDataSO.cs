using Sirenix.OdinInspector;
using TenMaker.Utility.SO;
using UnityEngine;

namespace TenMaker.Editor.Hub
{
    [CreateAssetMenu(menuName = "Ten Maker/Editor/Hub")]
    public class TenMakerHubDataSO : ScriptableObject
    {
        [Title("General")]
        [SerializeField, FilePath] public string gameManagerPath;
        
        [SerializeField, FilePath] public ColorsSO uiColorPresetPath;
    }
}