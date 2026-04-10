namespace TenMaker.Core.Input
{
    public interface IInputManager
    {
        public TMPlayerInput Player { get; }

        public TMPlayerInput GetPlayer(int playerIndex);
    }
}