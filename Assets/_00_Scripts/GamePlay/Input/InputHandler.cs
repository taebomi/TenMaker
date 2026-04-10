using TenMaker.Core.Input;

namespace TenMaker.Gameplay.Input
{
    public abstract class InputHandler
    {
        public TMPlayerInput PlayerInput { get; private set; }
        public abstract string MapName { get; }

        private bool _isEnabled;

        public InputHandler(TMPlayerInput playerInput)
        {
            PlayerInput = playerInput;
        }

        public void Initialize()
        {
            AddInputCallbacks();
        }

        public void Deinitialize()
        {
            RemoveInputCallbacks();
        }

        public void Enable()
        {
            if (_isEnabled) return;

            _isEnabled = true;
            PlayerInput.EnableMap(MapName);
        }

        public void Disable()
        {
            if (_isEnabled is false) return;

            _isEnabled = false;
            PlayerInput.DisableMap(MapName);
        }

        protected abstract void AddInputCallbacks();
        protected abstract void RemoveInputCallbacks();
    }
}