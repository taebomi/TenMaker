namespace TenMaker.Core.Scene
{
    public interface ISceneController
    {
        string SceneName { get; }
        
        public bool IsInitialized { get; }
    }
}