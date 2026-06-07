using _Project._Code.Features.Enemies;
using _Project._Code.Features.Level;
using _Project._Code.Services.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project._Code.Features.Factories
{
    public class EnemyFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly IStaticDataService _staticDataService;

        public EnemyFactory(IObjectResolver resolver, IStaticDataService staticDataService)
        {
            _resolver = resolver;
            _staticDataService = staticDataService;
        }

        public Enemy Create(Vector3 position, Transform target, Transform parent)
        {
            LevelConfig levelConfig = _staticDataService.GetLevelConfig();
            Enemy enemy = _resolver.Instantiate(_staticDataService.GetEnemyPrefab(), position, Quaternion.identity, parent);
            enemy.Initialize(target, levelConfig);
            return enemy;
        }
    }
}