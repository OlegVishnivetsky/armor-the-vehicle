using System.Linq;
using _Project._Code.Services.StaticData;
using Lean.Pool;
using UnityEngine;
using ZLinq;

namespace _Project._Code.Services.Vfx
{
    public class VfxService : IVfxService
    {
        private readonly IStaticDataService _staticDataService;

        public VfxService(IStaticDataService staticDataService) => _staticDataService = staticDataService;

        public Vfx PlayVfx(string name, Vector3 position, Transform parent = null)
        {
            VfxData vfxData = _staticDataService
                .GetVfxConfig().VFXDatas
                .AsValueEnumerable()
                .FirstOrDefault(fx => fx.Name == name);

            if (vfxData == null)
            {
                Debug.LogWarning($"[VFXService]: Effect with name {name} missing!");
                return null;
            }

            Vfx vfx = LeanPool.Spawn(vfxData.Prefab, position, Quaternion.identity, parent);
            vfx.Particle.Play();
            return vfx;
        }
    }
}