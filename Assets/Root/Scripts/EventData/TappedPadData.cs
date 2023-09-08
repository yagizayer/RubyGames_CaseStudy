// ClickedPadData.cs

using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Interfaces;
using Root.Scripts.Level;

namespace Root.Scripts.EventData
{
    public class TappedPadData : IPassableData, ISoundModifier
    {
        public Pad Pad { get; set; }
        public string Key { get; set; }
        public float Pitch01 { get; set; }
        public float Volume01 { get; set; }
    }
}