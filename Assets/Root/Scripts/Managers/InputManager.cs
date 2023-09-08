using Root.Scripts.Helpers.Extensions;
using Root.Scripts.Helpers.Interfaces;
using Root.Scripts.Helpers.Serialization;
using UnityEngine;

namespace Root.Scripts.Managers
{
    public sealed class InputManager : SingletonBase<InputManager>
    {
        [SerializeField]
        private ScriptableObject inputHandler;

        private IInputHandler _inputHandler;

        private void Update()
        {
            if (!_inputHandler.InputEnabled) return;
            _inputHandler.Update();
        }

        private void OnEnable()
        {
            OnValidate();
            _inputHandler.EnableInput();
        }

        private void OnDisable()
        {
            _inputHandler.DisableInput();
        }

        private void OnValidate()
        {
            if (!inputHandler.ValidateInterface(out IInputHandler handler)) return;
            _inputHandler = handler;
        }

        public void ChangeInputHandler(ScriptableObject newInputHandler)
        {
            if (!newInputHandler.ValidateInterface(out IInputHandler handler)) return;
            _inputHandler.DisableInput();
            inputHandler = newInputHandler;
            _inputHandler = handler;
            _inputHandler.EnableInput();
        }
    }
}