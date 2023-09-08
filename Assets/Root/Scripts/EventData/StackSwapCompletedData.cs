// StackSwapCompletedData.cs

using System.Collections.Generic;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Level;

namespace Root.Scripts.EventData
{
    public class StackSwapCompletedData : IPassableData
    {
        public Pad Pad { get; set; }
        public int NewStackSize { get; set; }
        public List<StackElement> SwappedElements { get; set; }
    }
}