using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using _Project._Code.Core.StateMachine;
using _Project._Code.Core.StateMachine.State;
using _Project._Code.Features.Level;
using _Project._Code.Features.UI.Panels;
using _Project._Code.Features.Vehicle;
using _Project._Code.Services.Input;
using _Project._Code.Services.Vfx;

namespace _Project._Code.Core.States
{
    public class LoseState : IEnterState, IExitState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IInputService _inputService;
        private readonly IVfxService _vfxService;
        private readonly LevelContext _levelContext;
        private readonly LosePanel _losePanel;

        private CancellationTokenSource _cancellation;
        
        private const float DelayBeforeShowPanel = 2f;
        
        public LoseState(
            IGameStateMachine stateMachine,
            IInputService inputService,
            IVfxService vfxService,
            LevelContext levelContext,
            LosePanel losePanel)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
            _vfxService = vfxService;
            _levelContext = levelContext;
            _losePanel = losePanel;
        }

        public void Enter()
        {
            _cancellation = new();
            _inputService.Disable();
            
            Car car = _levelContext.Car;
            car.PlayDeath();
            
            _vfxService.PlayVfx(Constants.Vfx.Explosion, car.transform.position);

            PlayLoseAsync().Forget();
        }

        public void Exit()
        {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
            _cancellation = null;
        }

        private async UniTaskVoid PlayLoseAsync()
        {
            try
            {
                await UniTask.WaitForSeconds(DelayBeforeShowPanel, cancellationToken: _cancellation.Token);

                _losePanel.Show();
                _stateMachine.SwitchTo<WaitForRestartState>();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error while LoseState: {e.Message}");
            }
        }
    }
}