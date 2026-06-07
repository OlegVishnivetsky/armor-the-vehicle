using _Project._Code.Features.Combat;
using UnityEngine;

namespace _Project._Code.Features.Vehicle
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "Game/Car Config")]
    public class CarConfig : ScriptableObject
    {
        [Header("Car")]
        public Car CarPrefab;
        public float MaxHealth = 100f;
        public float Speed = 8f;
        public float WheelRadius = 0.2f;
        
        [Header("Aim")]
        public float Sensitivity = 0.15f;
        public float SmoothTime = 0.08f;
        public float MinAngle = -90f;
        public float MaxAngle = 90f;
        
        [Header("Shooting")]
        public Bullet BulletPrefab;
        public float BulletSpeed;
        public float BulletDamage;
        public float BulletLifetime;
        public float FireInterval;
    }
}