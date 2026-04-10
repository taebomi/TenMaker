using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Player.Multiplay;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public abstract class CellBackground : MonoBehaviour
    {
        public abstract void SetPlayerArea(PlayerColor color);

        public void SetDefaultColor() => SetPlayerArea(PlayerColor.None);
    }
}