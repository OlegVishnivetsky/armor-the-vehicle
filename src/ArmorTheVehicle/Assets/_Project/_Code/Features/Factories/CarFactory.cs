using _Project._Code.Features.Vehicle;
using _Project._Code.Services.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project._Code.Features.Factories
{
    public class CarFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly IStaticDataService _staticDataService;

        public CarFactory(IObjectResolver resolver, IStaticDataService staticDataService)
        {
            _resolver = resolver;
            _staticDataService = staticDataService;
        }

        public Car Create(Transform spawnPoint)
        {
            CarConfig carConfig = _staticDataService.GetCarConfig();
            Car car = _resolver.Instantiate(
                carConfig.CarPrefab, 
                spawnPoint.position, 
                spawnPoint.rotation);
            car.Initialize(carConfig);
            
            return car;
        }
    }
}