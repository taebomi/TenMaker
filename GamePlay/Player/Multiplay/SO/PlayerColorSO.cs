using System.Collections.Generic;
using Sirenix.OdinInspector;
using TenMaker.Gameplay.Customization;
using UnityEngine;

namespace TenMaker.Gameplay.Player.Multiplay
{
    
    [CreateAssetMenu(fileName = "PlayerColor", menuName = "TenMaker/Data/Player Color")]
    public class PlayerColorSO : SerializedScriptableObject
    {
        [field:SerializeField] public Dictionary<PlayerColor, Color> Colors { get; private set; }
    }
}