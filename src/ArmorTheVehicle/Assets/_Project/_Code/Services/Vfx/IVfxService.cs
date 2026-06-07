using UnityEngine;

namespace _Project._Code.Services.Vfx
{
    public interface IVfxService
    {
        Vfx PlayVfx(string name, Vector3 position, Transform parent = null);
    }
}