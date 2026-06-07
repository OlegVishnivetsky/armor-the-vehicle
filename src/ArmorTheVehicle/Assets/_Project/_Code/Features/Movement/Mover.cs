using UnityEngine;

namespace _Project._Code.Features.Movement
{
    public class Mover
    {
        private Rigidbody _rigidbody;

        public void Initialize(Rigidbody rigidbody) => _rigidbody = rigidbody;
        
        public void Move(Vector3 direction, float speed, float deltaTime) =>
            _rigidbody.MovePosition(_rigidbody.position + direction * (speed * deltaTime));
    }
}