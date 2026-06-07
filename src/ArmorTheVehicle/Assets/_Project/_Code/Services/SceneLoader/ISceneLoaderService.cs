using Cysharp.Threading.Tasks;

namespace _Project._Code.Services.SceneLoader
{
    public interface ISceneLoaderService
    {
        UniTask LoadAsync(string sceneName);
    }
}