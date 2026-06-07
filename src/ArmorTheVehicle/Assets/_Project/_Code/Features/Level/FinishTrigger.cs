using System;
using _Project._Code.Features.Vehicle;
using R3;
using R3.Triggers;
using UnityEngine;

namespace _Project._Code.Features.Level
{
    [RequireComponent(typeof(Collider))]
    public class FinishTrigger : MonoBehaviour
    {
        private bool _reached;

        public event Action Reached;

        private void Start() =>
            this.OnTriggerEnterAsObservable()
                .Where(_ => !_reached)
                .Select(other => other.GetComponent<Car>())
                .Where(car => car != null)
                .Subscribe(_ =>
                {
                    _reached = true;
                    Reached?.Invoke();
                })
                .AddTo(this);
    }
}