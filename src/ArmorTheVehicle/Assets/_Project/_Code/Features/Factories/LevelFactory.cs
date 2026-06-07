using _Project._Code.Services.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project._Code.Features.Level
{
    public class LevelFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly IStaticDataService _staticDataService;

        public LevelFactory(
            IObjectResolver resolver, 
            IStaticDataService staticDataService)
        {
            _resolver = resolver;
            _staticDataService = staticDataService;
        }

        public LevelField Create() => 
            _resolver.Instantiate(_staticDataService.GetLevelConfig().LevelPrefab, Vector3.zero, Quaternion.identity);
    }
}