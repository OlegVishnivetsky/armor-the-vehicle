using System;
using UnityEngine;

namespace _Project._Code.Services.Audio
{
    [Serializable]
    public class AudioClipData
    {
        public string Name;
        public AudioClip Clip;
        
        [Range(0f, 1f)]
        public float Volume = 1f;
        
        [Range(0f, 0.5f)]
        public float PitchRandom;
    }
}