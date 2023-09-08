// SoundManager.cs

using System;
using System.Collections.Generic;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Extensions;
using Root.Scripts.Helpers.Interfaces;
using Root.Scripts.Helpers.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Root.Scripts.Managers
{
    public class SoundManager : SingletonBase<SoundManager>
    {
        [SerializeField]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private SerializableDictionary<string, Sound> sounds;

        private readonly List<AudioSource> _sources = new();

        private void Start()
        {
            for (var i = 0; i < 10; i++) CreateSource();

            foreach (var sound in sounds)
                if (sound.Value.startOnAwake)
                    PlayOneShot(sound.Key);
        }

        public void PlayOneShot(IPassableData rawData)
        {
            var data = rawData.To<ISoundModifier>();
            if (!sounds.TryGetValue(data.Key, out var sound)) return;

            var source = GetSource();
            source.clip = sound.clip;
            source.loop = sound.loop;
            sound.currentSource = source;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            source.pitch = data.Pitch01 == default
                ? Random.Range(sound.pitchRange.x, sound.pitchRange.y)
                : data.Pitch01.Remap(0, 1, sound.pitchRange.x, sound.pitchRange.y);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            source.volume = data.Volume01 == default
                ? Random.Range(sound.volumeRange.x, sound.volumeRange.y)
                : data.Volume01.Remap(0, 1, sound.volumeRange.x, sound.volumeRange.y);

            source.Play();
        }

        public void PlayOneShot(string key)
        {
            if (!sounds.TryGetValue(key, out var sound)) return;

            var source = GetSource();
            source.clip = sound.clip;
            source.loop = sound.loop;
            sound.currentSource = source;
            source.pitch = Random.Range(sound.pitchRange.x, sound.pitchRange.y);
            source.volume = Random.Range(sound.volumeRange.x, sound.volumeRange.y);
            source.PlayOneShot(sound.clip);
        }

        public void Stop(string key)
        {
            if (!sounds.TryGetValue(key, out var sound)) return;
            if (sound.currentSource == null) return;
            sound.currentSource.Stop();
        }

        private AudioSource GetSource()
        {
            foreach (var source in _sources)
                if (!source.isPlaying)
                    return source;

            return CreateSource();
        }

        private AudioSource CreateSource()
        {
            var sourceHolder = new GameObject("Source_" + _sources.Count, typeof(AudioSource));
            sourceHolder.transform.SetParent(transform);
            var source = sourceHolder.GetComponent<AudioSource>();
            _sources.Add(source);
            return source;
        }
    }
}