using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace _Project._Code.Features.Camera
{
    public class CameraDirector
    {
        private readonly CinemachineBrain _brain;
        private readonly List<CameraData> _cameras;
        
        public bool IsBlending => _brain.IsBlending;
        
        private const int InactivePriority = 0;
        private const int ActivePriority = 1;

        public CameraDirector(List<CameraData> cameras, CinemachineBrain brain)
        {
            _cameras = cameras;
            _brain = brain;
        }

        public void SwitchTo(CameraType type)
        {
            foreach (CameraData data in _cameras)
            {
                data.Camera.Priority = InactivePriority;
                
                if (data.Type == type)
                    data.Camera.Priority = ActivePriority;
            }
        }

        public void Bind(Transform player)
        {
            foreach (CameraData data in _cameras)
            {
                data.Camera.Follow = player;
                data.Camera.LookAt = player;
            }
        }
    }
}