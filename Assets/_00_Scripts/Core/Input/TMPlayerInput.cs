using System.Collections.Generic;
using TenMaker.RankMode;
using TenMaker.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TenMaker.Core.Input
{
    public class TMPlayerInput : MonoBehaviour
    {
        public Dictionary<string, InputActionMap> MapDict { get; private set; }
        public Dictionary<string, InputAction> ActionDict { get; private set; }
        
        [SerializeField] private PlayerInput playerInput;

        private void Awake()
        {
            MapDict = new Dictionary<string, InputActionMap>();
            ActionDict = new Dictionary<string, InputAction>();
        }

        public void Initialize()
        {
            var asset = playerInput.actions;
            foreach (var map in asset.actionMaps)
            {
                MapDict.Add(map.name, map);
                foreach (var action in map.actions)
                {
                    ActionDict.Add($"{map.name}/{action.name}", action);
                }
            }

            asset.Disable();
            playerInput.SwitchCurrentActionMap(InputConstants.Maps.UI);
        }

        public void EnableMap(string mapName)
        {
            if (MapDict.TryGetValue(mapName, out var map) is false)
            {
                Debug.LogError($"# TBMPlayerInput - EnableMap: {mapName} is not found.");
                return;
            }

            map.Enable();
        }

        public void DisableMap(string mapName)
        {
            if (MapDict.TryGetValue(mapName, out var map) is false)
            {
                Debug.LogError($"# TBMPlayerInput - DisableMap: {mapName} is not found.");
                return;
            }

            map.Disable();
        }

        public void SwitchToUI()
        {
            playerInput.SwitchCurrentActionMap(InputConstants.Maps.UI);
        }

        public bool TryGetAction(string actionName, out InputAction action)
        {
            if (ActionDict.TryGetValue(actionName, out action) is false)
            {
                TBMLog.HeaderError($"{actionName} is not found.");
                return false;
            }

            return true;
        }
    }
}