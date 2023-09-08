namespace Root.Scripts.Helpers.Interfaces
{
    public interface IInputHandler
    {
        bool InputEnabled { get; }
        void Update();
        void EnableInput();
        void DisableInput();
    }
}