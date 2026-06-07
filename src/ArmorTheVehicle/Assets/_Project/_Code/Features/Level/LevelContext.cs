using System.Collections.Generic;
using _Project._Code.Features.Enemies;
using _Project._Code.Features.Vehicle;
using UnityEngine;

namespace _Project._Code.Features.Level
{
    public class LevelContext
    {
        private readonly List<Enemy> _activeEnemies = new();

        public Car Car { get; private set; }
        public LevelField Field { get; private set; }
        public FinishTrigger Finish { get; private set; }

        public void SetLevel(LevelField field)
        {
            Field = field;
            Finish = field.Finish;
        }

        public void SetCar(Car car) => Car = car;

        public void Register(IReadOnlyList<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                _activeEnemies.Add(enemy);
                enemy.Died += OnEnemyDied;
            }
        }
        
        public void Cleanup()
        {
            foreach (Enemy enemy in _activeEnemies)
            {
                if (enemy != null)
                    enemy.Died -= OnEnemyDied;
            }

            _activeEnemies.Clear();

            if (Car != null)
                Object.Destroy(Car.gameObject);
            
            Car = null;

            if (Field != null)
                Object.Destroy(Field.gameObject);
            
            Field = null;
            Finish = null;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.Died -= OnEnemyDied;
            
            _activeEnemies.Remove(enemy);
            Object.Destroy(enemy.gameObject);
        }
    }
}