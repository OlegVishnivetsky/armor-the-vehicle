using System;
using _Project._Code.Core.StateMachine;
using _Project._Code.Core.StateMachine.State;
using _Project._Code.Features.Camera;
using _Project._Code.Features.UI.Panels;
using _Project._Code.Services.Input;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using CameraType = _Project._Code.Features.Camera.CameraType;

namespace _Project._Code.Core.States
{
    public class WaitForStartState : IEnterState, IExitState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IInputService _inputService;
        private readonly TransitionPanel _transitionPanel;
        private readonly CameraDirector _cameraDirector;

        public WaitForStartState(
            IGameStateMachine stateMachine,
            IInputService inputService,
            TransitionPanel transitionPanel,
            CameraDirector cameraDirector)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
            _transitionPanel = transitionPanel;
            _cameraDirector = cameraDirector;
        }

        public void Enter() => PrepareAsync().Forget();

        public void Exit() => _inputService.Tapped -= OnTapped;

        private async UniTaskVoid PrepareAsync()
        {
            try
            {
                _inputService.Disable();
                _cameraDirector.SwitchTo(CameraType.Starting);
            
                await UniTask.Yield();
                await UniTask.WaitWhile(() => _cameraDirector.IsBlending,
                    cancellationToken: Application.exitCancellationToken);
                await _transitionPanel.Hide().AsyncWaitForCompletion().AsUniTask();

                _inputService.Tapped += OnTapped;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error while WaitForStartState: {e.Message}");
            }
        }
        
        private void OnTapped() => _stateMachine.SwitchTo<GameLoopState>();
    }
}