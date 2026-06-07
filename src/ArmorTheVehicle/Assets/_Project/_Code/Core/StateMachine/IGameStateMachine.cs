using _Project._Code.Core.StateMachine.State;

namespace _Project._Code.Core.StateMachine
{
    public interface IGameStateMachine
    {
        IState CurrentState { get; }

        void RegisterState<TState>(IState state) where TState : class, IState;
        void SwitchTo<TState>() where TState : class, IState;
    }
}