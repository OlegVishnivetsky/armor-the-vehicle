using UnityEngine;

namespace _Project._Code.Features.Level
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public LevelField LevelPrefab;
        
        [Header("Enemy")]
        public float EnemyMaxHealth = 30f;
        public float EnemySpeed = 5f;
        public float EnemyTriggerRange = 12f;
        public float EnemyDamageToCar = 20f;
    }
}