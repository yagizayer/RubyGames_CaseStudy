// TouchData.cs

using System.Collections.Generic;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Interfaces;
using Root.Scripts.Level;
using Root.Scripts.Pooling;
using UnityEngine;

namespace Root.Scripts.EventData
{
    public class LimitReachedData : IPassableData,ISoundModifier
    {
        public Pools TargetPool { get; set; }
        public Pad Pad { get; set; }
        public string Key { get; set; }
        public float Pitch01 { get; set; }
        public float Volume01 { get; set; }
    }
}