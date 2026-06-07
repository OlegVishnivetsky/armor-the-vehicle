using _Project._Code.Features.Enemies;
using _Project._Code.Features.Level;
using _Project._Code.Features.Vehicle;
using _Project._Code.Services.Audio;
using _Project._Code.Services.Vfx;

namespace _Project._Code.Services.StaticData
{
    public interface IStaticDataService
    {
        void LoadAll();
        LevelConfig GetLevelConfig();
        VfxConfig GetVfxConfig();
        AudioConfig GetAudioConfig();
        CarConfig GetCarConfig();
        Enemy GetEnemyPrefab();
    }
}