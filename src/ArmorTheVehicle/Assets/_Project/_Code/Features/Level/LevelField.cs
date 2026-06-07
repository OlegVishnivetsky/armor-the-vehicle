using System.Collections.Generic;
using UnityEngine;

namespace _Project._Code.Features.Level
{
    public class LevelField : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private FinishTrigger _finish;
        [SerializeField] private List<Transform> _enemySpawnPoints = new();
        [SerializeField] private Transform _enemiesParent;

        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public FinishTrigger Finish => _finish;
        public IReadOnlyList<Transform> EnemySpawnPoints => _enemySpawnPoints;
        public Transform EnemiesParent => _enemiesParent != null ? _enemiesParent : transform;
    }
}