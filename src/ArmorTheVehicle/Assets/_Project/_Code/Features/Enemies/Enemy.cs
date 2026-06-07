using System;
using _Project._Code.Core;
using _Project._Code.Features.Combat;
using _Project._Code.Features.Feedback;
using _Project._Code.Features.Level;
using _Project._Code.Features.Movement;
using _Project._Code.Services.Vfx;
using MoreMountains.Feedbacks;
using R3;
using R3.Triggers;
using UnityEngine;
using VContainer;

namespace _Project._Code.Features.Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private MMF_Player _hitFeedback;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _turnSmoothing = 8f;

        private Rigidbody _rigidbody;
        private Mover _mover;
        private Health _health;
        private GlobalFeedbackPlayer _feedbackPlayer;
        private IVfxService _vfxService;
        private Transform _target;

        private float _speed;
        private float _triggerRange;
        private float _damageToCar;

        private bool _chasing;
        private bool _dead;

        public event Action<Enemy> Died;

        [Inject]
        public void Construct(
            Mover mover,
            Health health,
            GlobalFeedbackPlayer feedbackPlayer,
            IVfxService vfxService)
        {
            _mover = mover;
            _health = health;
            _feedbackPlayer = feedbackPlayer;
            _vfxService = vfxService;
        }

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        private void Start() =>
            this.OnTriggerEnterAsObservable()
                .Where(_ => !_dead)
                .Subscribe(other =>
                {
                    Rigidbody body = other.attachedRigidbody;

                    if (body == null || body.TryGetComponent<Enemy>(out _))
                        return;

                    if (body.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(_damageToCar);
                        Die();
                    }
                })
                .AddTo(this);

        private void FixedUpdate()
        {
            if (_dead || !_target)
                return;
            
            if (!_chasing)
            {
                if (DistanceToTarget() > _triggerRange)
                    return;

                StartChase();
            }

            Chase();
        }

        public void Initialize(Transform target, LevelConfig config)
        {
            _animator.SetRunning(false);
            _mover.Initialize(_rigidbody);
            _health.Initialize(config.EnemyMaxHealth);
            
            _target = target;
            _speed = config.EnemySpeed;
            _triggerRange = config.EnemyTriggerRange;
            _damageToCar = config.EnemyDamageToCar;

            _chasing = false;
            _dead = false;
        }

        public void TakeDamage(float amount)
        {
            if (_dead)
                return;

            StartChase();
            
            _health.TakeDamage(amount);
            _hitFeedback.PlayFeedbacks();
            _vfxService.PlayVfx(Constants.Vfx.SmallHit, new(transform.position.x, 
                transform.position.y + 1.5f, transform.position.z));

            if (_health.IsDead())
                Die();
        }

        private void StartChase()
        {
            _chasing = true;
            _animator.SetRunning(true);
        }

        private void Chase()
        {
            Vector3 toTarget = _target.position - _rigidbody.position;
            toTarget.y = 0f;
            Vector3 direction = toTarget.normalized;

            Quaternion look = Quaternion.LookRotation(direction);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, look, _turnSmoothing * Time.fixedDeltaTime);

            _mover.Move(direction, _speed, Time.fixedDeltaTime);
        }

        private float DistanceToTarget()
        {
            Vector3 delta = _target.position - _rigidbody.position;
            delta.y = 0f;
            return delta.magnitude;
        }

        private void Die()
        {
            if (_dead)
                return;

            _dead = true;
            _feedbackPlayer.PlayFeedback(FeedbackType.Kill);
            _vfxService.PlayVfx(Constants.Vfx.BigHit, new(transform.position.x, 
                transform.position.y + 1.5f, transform.position.z));
            Died?.Invoke(this);
        }
    }
}