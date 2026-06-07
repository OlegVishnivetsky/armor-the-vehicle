using _Project._Code.Core.StateMachine;
using _Project._Code.Core.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace _Project._Code.Core.Bootstrap
{
    public class Bootstrapper : IStartable
    {
        private readonly IGameStateMachine _stateMachine;

        public Bootstrapper(IGameStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Start() => _stateMachine.SwitchTo<BootState>();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ExecuteBootScene()
        {
            if (SceneManager.GetActiveScene().name == Constants.SceneNames.Boot)
                return;

            SceneManager.LoadScene(Constants.SceneNames.Boot);
        }
    }
}