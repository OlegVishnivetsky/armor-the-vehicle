using System.Collections.Generic;
using UnityEngine;

namespace _Project._Code.Services.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Game/Audio Config")]
    public class AudioConfig : ScriptableObject
    {
        public List<AudioClipData> Sounds;
    }
}