using _Project._Code.Core.StateMachine;
using _Project._Code.Core.StateMachine.State;
using _Project._Code.Features.Camera;
using _Project._Code.Features.Level;
using _Project._Code.Features.Vehicle;
using _Project._Code.Services.Input;

namespace _Project._Code.Core.States
{
    public class GameLoopState : IEnterState, IExitState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IInputService _inputService;
        private readonly CameraDirector _cameraDirector;
        private readonly LevelContext _context;

        public GameLoopState(
            IGameStateMachine gameStateMachine, 
            IInputService inputService,
            CameraDirector cameraDirector,
            LevelContext context)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _cameraDirector = cameraDirector;
            _context = context;
        }

        public void Enter()
        {
            Car car = _context.Car;
            car.StartMoving();
            car.StartFiring();
            car.Health.Died += OnCarDied;
            
            _cameraDirector.SwitchTo(CameraType.Main);
            _inputService.Enable();
            _context.Finish.Reached += OnFinishReached;
        }

        public void Exit()
        {
            Car car = _context.Car;
            car.StopMoving();
            car.StopFiring();
            car.Health.Died -= OnCarDied;
            
            _inputService.Disable();
            _context.Finish.Reached -= OnFinishReached;
        }

        private void OnCarDied() => _gameStateMachine.SwitchTo<LoseState>();

        private void OnFinishReached() => _gameStateMachine.SwitchTo<VictoryState>();
    }
}