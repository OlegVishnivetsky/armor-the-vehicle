using System.Collections.Generic;
using UnityEngine;

namespace _Project._Code.Features.Vehicle
{
    public class WheelDriver
    {
        private readonly List<Transform> _wheels;
        private readonly float _radius;

        public WheelDriver(List<Transform> wheels, float radius)
        {
            _wheels = wheels;
            _radius = Mathf.Max(radius, 0.0001f);
        }

        public void Spin(float speed, float deltaTime)
        {
            float degrees = speed / _radius * Mathf.Rad2Deg * deltaTime;

            for (int i = 0; i < _wheels.Count; i++)
                _wheels[i].Rotate(Vector3.right, degrees, Space.Self);
        }
    }
}