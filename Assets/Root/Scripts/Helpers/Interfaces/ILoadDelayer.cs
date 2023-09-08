// ILoadDelayer.cs

namespace Root.Scripts.Helpers.Interfaces
{
    public interface ILoadDelayer
    {
        public bool DelayCondition { get; }
    }
}