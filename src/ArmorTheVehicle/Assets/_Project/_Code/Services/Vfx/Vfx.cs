using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

namespace _Project._Code.Services.Vfx
{
    public class Vfx : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _particles;

        public ParticleSystem Particle => _particles[0];
        
        private void OnParticleSystemStopped() => LeanPool.Despawn(gameObject);
    }
}