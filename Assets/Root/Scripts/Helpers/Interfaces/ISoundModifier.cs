// ISoundModifier.cs

namespace Root.Scripts.Helpers.Interfaces
{
    public interface ISoundModifier
    {
        public string Key { get; set; }
        public float Pitch01 { get; set; }
        public float Volume01 { get; set; }
    }
}