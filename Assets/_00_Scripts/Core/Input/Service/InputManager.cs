using System.Collections.Generic;
using TenMaker.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TenMaker.Core.Input
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        [SerializeField] private PlayerInputManager inputManager;

        [field: SerializeField] public List<TMPlayerInput> Players { get; private set; }

        public TMPlayerInput Player => Players[0];

        #region Initialization

        public void Initialize()
        {
            TMInputManager.Initialize(this);
            
            Player.Initialize();
        }

        public void Deinitialize()
        {
            TMInputManager.Deinitialize(this);
        }

        #endregion


        public TMPlayerInput GetPlayer(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex >= Players.Count)
            {
                TBMLog.HeaderError($"Player index({playerIndex}) is out of range.\n Current Players : {Players.Count}");
                return null;
            }

            return Players[playerIndex];
        }
    }
}