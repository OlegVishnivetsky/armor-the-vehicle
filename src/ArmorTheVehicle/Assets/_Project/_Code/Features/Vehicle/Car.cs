using System.Collections.Generic;
using _Project._Code.Features.Combat;
using _Project._Code.Features.Feedback;
using _Project._Code.Features.Movement;
using _Project._Code.Features.UI.HealthBar;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace _Project._Code.Features.Vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class Car : MonoBehaviour, IDamageable
    {
        [SerializeField] private HealthBar _healthBar;
        
        [Header("Feedbacks")]
        [SerializeField] private MMF_Player _hitFeedback;
        [SerializeField] private MMF_Player _deathFeedback;
        
        [Header("Car Related")]
        [SerializeField] private Turret _turret;
        [SerializeField] private List<Transform> _wheels;

        private Rigidbody _rigidbody;
        private Mover _mover;
        private Health _health;
        private WheelDriver _wheelDriver;
        private GlobalFeedbackPlayer _feedbackPlayer;
        
        private float _speed;
        private bool _moving;

        public Health Health => _health;

        [Inject]
        public void Construct(Mover mover, Health health, GlobalFeedbackPlayer feedbackPlayer)
        {
            _mover = mover;
            _health = health;
            _feedbackPlayer = feedbackPlayer;
        }
        
        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        private void Update()
        {
            if (!_moving)
                return;
            
            _wheelDriver.Spin(_speed, Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (!_moving)
                return;
            
            _mover.Move(Vector3.forward, _speed, Time.fixedDeltaTime);
        }

        public void Initialize(CarConfig config)
        {
            _speed = config.Speed;
            _wheelDriver = new(_wheels, config.WheelRadius);

            _health.Initialize(config.MaxHealth);
            _mover.Initialize(_rigidbody);
            _turret.Initialize(config);
            _healthBar.Bind(_health);
        }

        public void StartMoving()
        {
            _moving = true;
            _healthBar.Reveal();
        }

        public void StopMoving() => _moving = false;

        public void StartFiring() => _turret.StartFiring();

        public void StopFiring() => _turret.StopFiring();

        public void PlayDeath()
        {
            _deathFeedback.PlayFeedbacks();
            _feedbackPlayer.PlayFeedback(FeedbackType.Death);
        }

        public void TakeDamage(float amount)
        {
            _health.TakeDamage(amount);
            _hitFeedback.PlayFeedbacks();
            _feedbackPlayer.PlayFeedback(FeedbackType.DamageTaken);
        }
    }
}