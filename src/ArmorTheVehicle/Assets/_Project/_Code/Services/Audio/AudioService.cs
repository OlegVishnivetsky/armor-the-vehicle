using System.Collections.Generic;
using _Project._Code.Services.StaticData;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Project._Code.Services.Audio
{
    public class AudioService : ISoundService
    {
        private readonly Dictionary<string, AudioClipData> _soundClips = new();

        private AudioSource _source;

        public AudioService(IStaticDataService staticDataService)
        {
            foreach (AudioClipData data in staticDataService.GetAudioConfig().Sounds)
                _soundClips[data.Name] = data;
        }

        public void PlaySound(string name)
        {
            if (!_source)
                CreateSource();

            if (!_soundClips.TryGetValue(name, out AudioClipData data))
            {
                Debug.LogWarning($"Sound '{name}' missing!");
                return;
            }

            float pitch = data.PitchRandom <= 0f ? 1f : 1f + Random.Range(-data.PitchRandom, data.PitchRandom);

            _source.pitch = pitch;
            _source.PlayOneShot(data.Clip, data.Volume);
        }

        private void CreateSource()
        {
            GameObject host = new("AudioService");
            Object.DontDestroyOnLoad(host);

            _source = host.AddComponent<AudioSource>();
            _source.playOnAwake = false;
        }
    }
}