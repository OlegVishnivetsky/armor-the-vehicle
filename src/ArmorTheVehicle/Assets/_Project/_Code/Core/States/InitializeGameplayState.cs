using System;
using System.Collections.Generic;
using _Project._Code.Core.StateMachine;
using _Project._Code.Core.StateMachine.State;
using _Project._Code.Features.Camera;
using _Project._Code.Features.Enemies;
using _Project._Code.Features.Factories;
using _Project._Code.Features.Level;
using _Project._Code.Features.UI.Panels;
using _Project._Code.Features.Vehicle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project._Code.Core.States
{
    public class InitializeGameplayState : IEnterState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly LevelFactory _levelFactory;
        private readonly CarFactory _carFactory;
        private readonly EnemyFactory _enemyFactory;
        private readonly TransitionPanel _transitionPanel;
        private readonly LevelContext _context;
        private readonly CameraDirector _cameraDirector;

        public InitializeGameplayState(
            IGameStateMachine stateMachine,
            LevelFactory levelFactory,
            CarFactory carFactory,
            EnemyFactory enemyFactory,
            TransitionPanel transitionPanel,
            LevelContext context,
            CameraDirector cameraDirector)
        {
            _stateMachine = stateMachine;
            _levelFactory = levelFactory;
            _carFactory = carFactory;
            _enemyFactory = enemyFactory;
            _transitionPanel = transitionPanel;
            _context = context;
            _cameraDirector = cameraDirector;
        }

        public void Enter() => RebuildAsync().Forget();

        private async UniTaskVoid RebuildAsync()
        {
            try
            {
                await _transitionPanel.Show().AsyncWaitForCompletion().AsUniTask();

                _context.Cleanup();

                LevelField field = _levelFactory.Create();
                Car car = _carFactory.Create(field.PlayerSpawnPoint);

                _context.SetLevel(field);
                _context.SetCar(car);
                _cameraDirector.Bind(car.transform);

                SpawnEnemies(field, car.transform);

                _stateMachine.SwitchTo<WaitForStartState>();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error while InitializeGameplayState: {e.Message}");
            }
        }

        private void SpawnEnemies(LevelField field, Transform target)
        {
            List<Enemy> spawned = new(field.EnemySpawnPoints.Count);

            foreach (Transform point in field.EnemySpawnPoints)
                spawned.Add(_enemyFactory.Create(point.position, target, field.EnemiesParent));

            _context.Register(spawned);
        }
    }
}