using Lean.Pool;
using UnityEngine;

namespace _Project._Code.Features.Combat
{
    [RequireComponent(typeof(Collider))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;
        
        private Vector3 _direction;
        
        private float _speed;
        private float _damage;
        private float _lifeRemaining;
        private bool _live;

        private void Update()
        {
            if (!_live)
                return;

            transform.position += _direction * (_speed * Time.deltaTime);

            _lifeRemaining -= Time.deltaTime;
            
            if (_lifeRemaining <= 0f)
                ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_live)
                return;

            if (other.TryGetComponent(out IDamageable damageable) ||
                (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out damageable)))
            {
                damageable.TakeDamage(_damage);
                ReturnToPool();
            }
        }

        public void Launch(Vector3 direction, float speed, float damage, float lifetime)
        {
            _trail.Clear();
            _direction = direction.normalized;
            _speed = speed;
            _damage = damage;
            _lifeRemaining = lifetime;
            _live = true;
            
            transform.rotation = Quaternion.LookRotation(_direction);
        }

        private void ReturnToPool()
        {
            _live = false;
            LeanPool.Despawn(gameObject);
        }
    }
}