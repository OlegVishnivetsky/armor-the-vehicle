using _Project._Code.Core.Bootstrap;
using _Project._Code.Core.StateMachine;
using _Project._Code.Core.StateMachine.Factory;
using _Project._Code.Core.States;
using _Project._Code.Features.UI.Panels;
using _Project._Code.Services.Audio;
using _Project._Code.Services.Input;
using _Project._Code.Services.SceneLoader;
using _Project._Code.Services.StaticData;
using _Project._Code.Services.Vfx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project._Code.Core.DI
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private TransitionPanel _transitionPanel;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterUI(builder);
            RegisterInputService(builder);
            RegisterStaticDataService(builder);
            RegisterVfxService(builder);
            RegisterAudioService(builder);
            RegisterSceneLoaderService(builder);
            RegisterStateMachine(builder);
            
            builder.RegisterEntryPoint<Bootstrapper>();
        }

        private void RegisterUI(IContainerBuilder builder) => builder.RegisterInstance(_transitionPanel);

        private void RegisterInputService(IContainerBuilder builder) =>
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
        
        private void RegisterStaticDataService(IContainerBuilder builder) => 
            builder.Register<IStaticDataService, StaticDataService>(Lifetime.Singleton);
        
        private void RegisterVfxService(IContainerBuilder builder) => 
            builder.Register<IVfxService, VfxService>(Lifetime.Singleton);
 
        private void RegisterAudioService(IContainerBuilder builder) => 
            builder.Register<AudioService>(Lifetime.Singleton).AsImplementedInterfaces();
        
        private void RegisterSceneLoaderService(IContainerBuilder builder) => 
            builder.Register<ISceneLoaderService, SceneLoaderService>(Lifetime.Singleton);
        
        private void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<BootState>(Lifetime.Singleton);
            builder.Register<StateFactory>(Lifetime.Singleton);
            builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}