using System;
using System.Threading;
using _Project._Code.Core.StateMachine;
using _Project._Code.Core.StateMachine.State;
using _Project._Code.Features.Camera;
using _Project._Code.Features.Level;
using _Project._Code.Features.UI.Panels;
using _Project._Code.Features.Vehicle;
using _Project._Code.Services.Input;
using _Project._Code.Services.Vfx;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CameraType = _Project._Code.Features.Camera.CameraType;

namespace _Project._Code.Core.States
{
    public class VictoryState : IEnterState, IExitState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IInputService _inputService;
        private readonly IVfxService _vfxService;
        private readonly VictoryPanel _victoryPanel;
        private readonly LevelContext _levelContext;
        private readonly CameraDirector _cameraDirector;

        private CancellationTokenSource _cancellation;

        private const float DelayBeforeShowPanel = 2f;
        
        public VictoryState(
            IGameStateMachine gameStateMachine, 
            IInputService inputService,
            IVfxService vfxService,
            VictoryPanel victoryPanel,
            LevelContext levelContext,
            CameraDirector cameraDirector)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _vfxService = vfxService;
            _victoryPanel = victoryPanel;
            _levelContext = levelContext;
            _cameraDirector = cameraDirector;
        }

        public void Enter()
        {
            _cancellation = new();
            _inputService.Disable();
            _cameraDirector.SwitchTo(CameraType.Final);
            
            Car car = _levelContext.Car;
            _vfxService.PlayVfx(Constants.Vfx.Confetti, car.transform.position);
            
            PlayVictoryAsync().Forget();
        }

        public void Exit()
        {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
            _cancellation = null;
        }

        private async UniTaskVoid PlayVictoryAsync()
        {
            try
            {
                await UniTask.WaitForSeconds(DelayBeforeShowPanel, cancellationToken: _cancellation.Token);

                _victoryPanel.Show();
                _gameStateMachine.SwitchTo<WaitForRestartState>();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error while VictoryState: {e.Message}");
            }
        }
    }
}
