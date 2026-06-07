using System;

namespace _Project._Code.Services.Input
{
    public interface IInputService
    {
        float AimDelta { get; }
        bool IsPressing { get; }

        event Action Tapped;

        void Enable();
        void Disable();
    }
}