// Sound.cs

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Root.Scripts.Helpers.Serialization
{
    [Serializable]
    public class Sound
    {
        [SerializeField]
        public AudioClip clip;

        [SerializeField]
        public bool loop;

        [SerializeField]
        public bool startOnAwake;

        [SerializeField]
        [LabelWidth(75)]
        [MinMaxSlider(-1, 3, true)]
        public Vector2 pitchRange = new(1, 1);

        [SerializeField]
        [LabelWidth(75)]
        [MinMaxSlider(-1, 3, true)]
        public Vector2 volumeRange = new(1, 1);

        [ReadOnly]
        public AudioSource currentSource = null;
    }
}