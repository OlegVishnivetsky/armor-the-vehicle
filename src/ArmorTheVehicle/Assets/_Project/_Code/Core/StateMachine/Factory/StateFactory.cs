using _Project._Code.Core.StateMachine.State;
using VContainer;

namespace _Project._Code.Core.StateMachine.Factory
{
    public class StateFactory
    {
        private readonly IObjectResolver _resolver;

        public StateFactory(IObjectResolver resolver) => _resolver = resolver;

        public TState Create<TState>() where TState : class, IState => _resolver.Resolve<TState>();
    }
}