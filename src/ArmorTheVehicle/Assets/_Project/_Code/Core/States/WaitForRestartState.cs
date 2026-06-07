using _Project._Code.Core.StateMachine;
using _Project._Code.Core.StateMachine.State;
using _Project._Code.Features.UI.Panels;
using _Project._Code.Services.Input;

namespace _Project._Code.Core.States
{
    public class WaitForRestartState : IEnterState, IExitState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IInputService _inputService;
        private readonly LosePanel _losePanel;
        private readonly VictoryPanel _victoryPanel;

        public WaitForRestartState(
            IGameStateMachine stateMachine,
            IInputService inputService,
            LosePanel losePanel,
            VictoryPanel victoryPanel)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
            _losePanel = losePanel;
            _victoryPanel = victoryPanel;
        }

        public void Enter()
        {
            _inputService.Disable();
            _inputService.Tapped += OnTapped;
        }

        public void Exit()
        {
            _inputService.Tapped -= OnTapped;
            
            if (_losePanel == null || _victoryPanel == null)
                return;
            
            _losePanel.Hide();
            _victoryPanel.Hide();
        }

        private void OnTapped() => _stateMachine.SwitchTo<InitializeGameplayState>();
    }
}