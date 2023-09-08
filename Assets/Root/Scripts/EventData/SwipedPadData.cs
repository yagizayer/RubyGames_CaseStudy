// ClickedPadData.cs

using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Interfaces;
using Root.Scripts.Level;
using UnityEngine;

namespace Root.Scripts.EventData
{
    public class SwipedPadData : IPassableData, ISoundModifier
    {
        public Pad StartPad { get; set; }
        public Pad EndPad { get; set; }
        public Vector3 SwipeDirectionInWorld { get; set; }
        public string Key { get; set; }
        public float Pitch01 { get; set; }
        public float Volume01 { get; set; }
    }
}