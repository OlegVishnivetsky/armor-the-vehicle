using System.Collections.Generic;
using UnityEngine;

namespace _Project._Code.Services.Vfx
{
    [CreateAssetMenu(fileName = "VfxConfig", menuName = "Game/Vfx Config")]
    public class VfxConfig : ScriptableObject
    {
        public List<VfxData> VFXDatas;
    }
}