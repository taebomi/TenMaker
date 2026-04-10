using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TenMaker.Core.Scene
{
    public class SceneManager : MonoBehaviour, ISceneService
    {
        public void Initialize() => TMSceneService.Initialize(this);
        public void Deinitialize() => TMSceneService.Deinitialize(this);

        public UniTask WaitForSceneReady() => UniTask.CompletedTask;

        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneWithMultiplay(string sceneName, AsyncOperation asyncOperation)
        {
            asyncOperation.allowSceneActivation = true;
        }
    }
}
