using System;
using R3;

namespace _Project._Code.Features.Combat
{
    public class Health
    {
        private readonly ReactiveProperty<float> _current = new();
        private float _max;
        
        public ReadOnlyReactiveProperty<float> Current => _current;
        public float Max => _max;

        public event Action Died;

        public void Initialize(float max)
        {
            _max = max;
            _current.Value = max;
        }
        
        public void TakeDamage(float amount)
        {
            if (IsDead())
                return;

            _current.Value = Math.Max(0f, _current.Value - amount);

            if (IsDead())
                Died?.Invoke();
        }
        
        public bool IsDead() => _current.Value <= 0f;
    }
}
