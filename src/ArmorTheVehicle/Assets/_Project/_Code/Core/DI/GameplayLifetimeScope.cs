using System.Collections.Generic;
using _Project._Code.Core.StateMachine;
using _Project._Code.Core.States;
using _Project._Code.Features.Camera;
using _Project._Code.Features.Combat;
using _Project._Code.Features.Factories;
using _Project._Code.Features.Feedback;
using _Project._Code.Features.Level;
using _Project._Code.Features.Movement;
using _Project._Code.Features.UI.Panels;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project._Code.Core.DI
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private CinemachineBrain _brain;
        [SerializeField] private List<CameraData> _cameras;
        [SerializeField] private List<FeedbackData> _feedbacks;

        [Space(10f)]
        [SerializeField] private VictoryPanel _victoryPanel;
        [SerializeField] private LosePanel _losePanel;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterLevelContext(builder);
            RegisterFactories(builder);
            RegisterMover(builder);
            RegisterHealth(builder);
            RegisterCameraDirector(builder);
            RegisterFeedbackPlayer(builder);
            RegisterUI(builder);
            RegisterStateMachine(builder);
        }

        private void Start() => RegisterStatesAndBootGameplay();

        private void RegisterLevelContext(IContainerBuilder builder) => builder.Register<LevelContext>(Lifetime.Singleton);

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<CarFactory>(Lifetime.Singleton);
            builder.Register<EnemyFactory>(Lifetime.Singleton);
            builder.Register<LevelFactory>(Lifetime.Singleton);
        }

        private void RegisterMover(IContainerBuilder builder) => builder.Register<Mover>(Lifetime.Transient);

        private void RegisterHealth(IContainerBuilder builder) => builder.Register<Health>(Lifetime.Transient);

        private void RegisterCameraDirector(IContainerBuilder builder)
        {
            builder.RegisterInstance(_cameras);
            builder.RegisterInstance(_brain);
            builder.Register<CameraDirector>(Lifetime.Singleton);
        }

        private void RegisterFeedbackPlayer(IContainerBuilder builder) => 
            builder.Register<GlobalFeedbackPlayer>(Lifetime.Singleton).WithParameter(_feedbacks);

        private void RegisterUI(IContainerBuilder builder)
        {
            builder.RegisterInstance(_victoryPanel);
            builder.RegisterInstance(_losePanel);
        }

        private void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<InitializeGameplayState>(Lifetime.Singleton);
            builder.Register<WaitForStartState>(Lifetime.Singleton);
            builder.Register<GameLoopState>(Lifetime.Singleton);
            builder.Register<VictoryState>(Lifetime.Singleton);
            builder.Register<LoseState>(Lifetime.Singleton);
            builder.Register<WaitForRestartState>(Lifetime.Singleton);
        }

        private void RegisterStatesAndBootGameplay()
        {
            IGameStateMachine stateMachine = Parent.Container.Resolve<IGameStateMachine>();
            InitializeGameplayState initializeState = Container.Resolve<InitializeGameplayState>();
            WaitForStartState waitForStartState = Container.Resolve<WaitForStartState>();
            GameLoopState gameLoopState = Container.Resolve<GameLoopState>();
            VictoryState victoryState = Container.Resolve<VictoryState>();
            LoseState loseState = Container.Resolve<LoseState>();
            WaitForRestartState waitForRestartState = Container.Resolve<WaitForRestartState>();
            
            stateMachine.RegisterState<InitializeGameplayState>(initializeState);
            stateMachine.RegisterState<WaitForStartState>(waitForStartState);
            stateMachine.RegisterState<GameLoopState>(gameLoopState);
            stateMachine.RegisterState<VictoryState>(victoryState);
            stateMachine.RegisterState<LoseState>(loseState);
            stateMachine.RegisterState<WaitForRestartState>(waitForRestartState);
            
            stateMachine.SwitchTo<InitializeGameplayState>();
        }
    }
}