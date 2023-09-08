// StackMergeCompletedData.cs

using System.Collections.Generic;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Interfaces;
using Root.Scripts.Level;

namespace Root.Scripts.EventData
{
    public class StackMergeCompletedData : IPassableData, ISoundModifier
    {
        public Pad Pad { get; set; }
        public int NewStackSize { get; set; }
        public List<StackElement> NewElements { get; set; }
        public List<StackElement> RemovedElements { get; set; }
        public string Key { get; set; }
        public float Pitch01 { get; set; }
        public float Volume01 { get; set; }
    }
}