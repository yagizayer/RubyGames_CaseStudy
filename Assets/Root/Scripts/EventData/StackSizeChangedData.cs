// StackSizeChangedData.cs

using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Level;

namespace Root.Scripts.EventData
{
    public class StackSizeChangedData : IPassableData
    {
        public Pad Pad { get; set; }
        public int NewStackSize { get; set; }
        public StackElement NewElement { get; set; }
    }
}