// TouchData.cs

using Root.Scripts.EventHandling.BasicPassableData;
using UnityEngine;

namespace Root.Scripts.EventData
{
    public class TouchData : IPassableData
    {
        public TouchPhase Phase { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 DeltaPosition { get; set; }
        public float Radius { get; set; }
        public float Pressure { get; set; }
    }
}