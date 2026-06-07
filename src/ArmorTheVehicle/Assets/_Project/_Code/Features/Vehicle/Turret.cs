using _Project._Code.Core;
using _Project._Code.Features.Combat;
using _Project._Code.Services.Audio;
using _Project._Code.Services.Input;
using Lean.Pool;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace _Project._Code.Features.Vehicle
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private MMSpringScale _scaleFeedback;
        [SerializeField] private Transform _rotationPoint;
        [SerializeField] private Transform _shootPoint;
        
        private IInputService _inputService;
        private ISoundService _soundService;
        private Bullet _bulletPrefab;

        private float _bulletSpeed;
        private float _bulletDamage;
        private float _bulletLifetime;
        private float _fireInterval;
        
        private float _sensitivity;
        private float _smoothTime;
        private float _minAngle;
        private float _maxAngle;

        private float _targetAngle;
        private float _currentAngle;
        private float _angularVelocity;

        private bool _firing;
        private float _fireCooldown;

        [Inject]
        public void Construct(IInputService inputService, ISoundService soundService)
        {
            _inputService = inputService;
            _soundService = soundService;
        }

        private void Update()
        {
            UpdateRotation();
            UpdateFiring();
        }

        public void Initialize(CarConfig config)
        {
            _bulletPrefab = config.BulletPrefab;
            
            _bulletSpeed = config.BulletSpeed;
            _bulletDamage = config.BulletDamage;
            _bulletLifetime = config.BulletLifetime;
            _fireInterval = config.FireInterval;

            _sensitivity = config.Sensitivity;
            _smoothTime = config.SmoothTime;
            _minAngle = config.MinAngle;
            _maxAngle = config.MaxAngle;
        }

        public void StartFiring()
        {
            _firing = true;
            _fireCooldown = 0f;
        }

        public void StopFiring() => _firing = false;

        private void UpdateRotation()
        {
            _targetAngle = Mathf.Clamp(_targetAngle + _inputService.AimDelta * _sensitivity, _minAngle, _maxAngle);
            _currentAngle = Mathf.SmoothDampAngle(_currentAngle, _targetAngle, ref _angularVelocity, _smoothTime);
            _rotationPoint.localRotation = Quaternion.Euler(0f, _currentAngle, 0f);
        }

        private void UpdateFiring()
        {
            _fireCooldown -= Time.deltaTime;
            
            if (!_firing || !_inputService.IsPressing)
                return;
            
            if (_fireCooldown > 0f)
                return;
            
            _fireCooldown = _fireInterval;
            Fire();
        }

        private void Fire()
        {
            Bullet bullet = LeanPool.Spawn(_bulletPrefab, _shootPoint.position, Quaternion.identity);
            bullet.Launch(_shootPoint.forward, _bulletSpeed, _bulletDamage, _bulletLifetime);
            
            _scaleFeedback.BumpRandom();
            _soundService.PlaySound(Constants.Sounds.Shoot);
        }
    }
}