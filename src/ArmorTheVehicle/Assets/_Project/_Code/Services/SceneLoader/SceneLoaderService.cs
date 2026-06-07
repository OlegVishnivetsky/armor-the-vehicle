using Cysharp.Threading.Tasks;
using _Project._Code.Features.UI.Panels;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace _Project._Code.Services.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly TransitionPanel _transitionPanel;

        public SceneLoaderService(TransitionPanel transitionPanel) => _transitionPanel = transitionPanel;

        public async UniTask LoadAsync(string sceneName)
        {
            await _transitionPanel.Show().AsyncWaitForCompletion().AsUniTask();

            await UniTask.Yield();
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            await UniTask.Yield();

            await _transitionPanel.Hide().AsyncWaitForCompletion().AsUniTask();
        }
    }
}