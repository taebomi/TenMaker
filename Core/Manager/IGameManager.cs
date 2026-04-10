using Cysharp.Threading.Tasks;

namespace TenMaker.Core
{
    public interface IGameManager
    {
        bool IsInitialized { get; }

        UniTask WaitForInitializationCompletedAsync();
        void QuitGame();
    }
}