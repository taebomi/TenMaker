using System;
using UnityEngine;

namespace TenMaker.Core.Input
{
    public abstract class InputHandler : MonoBehaviour
    {
        public TMPlayerInput PlayerInput { get; private set; }
        public abstract string MapName { get; }

        private void OnDisable()
        {
            RemoveCallbacks();
        }

        private void OnDestroy()
        {
            PlayerInput = null;
        }

        public void Setup(TMPlayerInput playerInput)
        {
            PlayerInput = playerInput;
            AddCallbacks();
        }

        public void Enable()
        {
            PlayerInput.EnableMap(MapName);
        }

        public void Disable()
        {
            PlayerInput.DisableMap(MapName);
        }

        protected abstract void AddCallbacks();
        protected abstract void RemoveCallbacks();
    }
}