using System;
using System.Collections.Generic;
using _Project._Code.Core.StateMachine.Factory;
using _Project._Code.Core.StateMachine.State;

namespace _Project._Code.Core.StateMachine
{
    public class GameStateMachine : IGameStateMachine, IDisposable
    {
        private readonly Dictionary<Type, IState> _states = new();
        private readonly StateFactory _stateFactory;

        public IState CurrentState { get; private set; }

        public GameStateMachine(StateFactory stateFactory) => _stateFactory = stateFactory;

        public void RegisterState<TState>(IState state) where TState : class, IState
        {
            if (_states.ContainsKey(typeof(TState)))
                return;
            
            _states.Add(typeof(TState), state);
        }
        
        public void SwitchTo<TState>() where TState : class, IState
        {
            if (CurrentState is IExitState exiting)
                exiting.Exit();

            CurrentState = GetOrCreate<TState>();

            if (CurrentState is IEnterState entering)
                entering.Enter();
        }

        public void Dispose()
        {
            if (CurrentState is IExitState exiting)
                exiting.Exit();
        }

        private TState GetOrCreate<TState>() where TState : class, IState
        {
            if (_states.TryGetValue(typeof(TState), out IState cached))
                return (TState)cached;

            TState created = _stateFactory.Create<TState>();
            _states.Add(typeof(TState), created);
            return created;
        }
    }
}