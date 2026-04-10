using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TenMaker.Core.Scene
{
    public interface ISceneService
    {
        /// <summary>
        /// 씬 진행 가능 상태까지 대기
        /// </summary>
        UniTask WaitForSceneReady();

        void LoadScene(string sceneName);

        /// <summary>
        /// 멀티플레이 전용. NetworkManager.SceneManager.OnLoad에서 받은 AsyncOperation 전달.
        /// </summary>
        void LoadSceneWithMultiplay(string sceneName, AsyncOperation asyncOperation);
    }
}
