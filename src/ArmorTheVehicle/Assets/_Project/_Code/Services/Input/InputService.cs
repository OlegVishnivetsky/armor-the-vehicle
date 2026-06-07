using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace _Project._Code.Services.Input
{
    public class InputService : IInputService, ITickable
    {
        private bool _enabled = true;

        public float AimDelta { get; private set; }
        public bool IsPressing { get; private set; }

        public event Action Tapped;

        public void Enable() => _enabled = true;

        public void Disable()
        {
            _enabled = false;
            AimDelta = 0f;
        }

        public void Tick()
        {
            Pointer pointer = Pointer.current;
            
            if (pointer == null)
            {
                AimDelta = 0f;
                IsPressing = false;
                return;
            }

            if (pointer.press.wasPressedThisFrame)
                Tapped?.Invoke();

            bool held = pointer.press.isPressed;
            IsPressing = _enabled && held;
            AimDelta = _enabled && held ? pointer.delta.ReadValue().x : 0f;
        }
    }
}