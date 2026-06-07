using _Project._Code.Core;
using _Project._Code.Features.Enemies;
using _Project._Code.Features.Level;
using _Project._Code.Features.Vehicle;
using _Project._Code.Services.Audio;
using _Project._Code.Services.Vfx;
using UnityEngine;

namespace _Project._Code.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private LevelConfig _levelConfig;
        private VfxConfig _vfxConfig;
        private AudioConfig _audioConfig;
        private CarConfig _carConfig;
        private Enemy _enemyPrefab;

        public void LoadAll()
        {
            _levelConfig = Resources.Load<LevelConfig>(Constants.ResourcePaths.LevelConfig);
            _vfxConfig = Resources.Load<VfxConfig>(Constants.ResourcePaths.VfxConfig);
            _audioConfig = Resources.Load<AudioConfig>(Constants.ResourcePaths.AudioConfig);
            _carConfig = Resources.Load<CarConfig>(Constants.ResourcePaths.CarConfig);
            _enemyPrefab = Resources.Load<Enemy>(Constants.ResourcePaths.EnemyPrefab);
        }

        public LevelConfig GetLevelConfig() => _levelConfig;

        public VfxConfig GetVfxConfig() => _vfxConfig;
        
        public AudioConfig GetAudioConfig() => _audioConfig;

        public CarConfig GetCarConfig() => _carConfig;

        public Enemy GetEnemyPrefab() => _enemyPrefab;
    }
}