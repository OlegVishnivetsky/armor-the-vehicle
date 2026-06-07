using _Project._Code.Core.StateMachine.State;
using _Project._Code.Services.SceneLoader;
using _Project._Code.Services.StaticData;
using Cysharp.Threading.Tasks;

namespace _Project._Code.Core.States
{
    public class BootState : IEnterState
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IStaticDataService _staticDataService;

        public BootState(ISceneLoaderService sceneLoaderService, IStaticDataService staticDataService)
        {
            _sceneLoaderService = sceneLoaderService;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            _staticDataService.LoadAll();
            _sceneLoaderService.LoadAsync(Constants.SceneNames.Gameplay).Forget();
        }
    }
}