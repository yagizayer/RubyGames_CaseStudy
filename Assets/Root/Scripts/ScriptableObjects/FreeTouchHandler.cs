using System.Collections.Generic;
using Root.Scripts.EventData;
using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Interfaces;
using Root.Scripts.Helpers.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Root.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FreeTouchHandler", menuName = "Handlers/FreeTouchHandler")]
    public class FreeTouchHandler : ScriptableObject, IInputHandler
    {
        [SerializeField]
        private bool disableOnUI = true;

        public bool InputEnabled { get; private set; }

        //--------------------------------------------------------------------------------------------------------------

        public void Update() => GetTouch();

        public void EnableInput() => InputEnabled = true;

        public void DisableInput() => InputEnabled = false;

        //--------------------------------------------------------------------------------------------------------------

        private void GetTouch()
        {
            if (!InputEnabled) return;
            if (Input.touchCount == 0) return;
            if (disableOnUI && IsPointerOverUIObject()) return;

            var touch = Input.GetTouch(0);
            var touchData = new TouchData
            {
                Phase = touch.phase,
                Position = touch.position,
                DeltaPosition = touch.deltaPosition,
                Radius = touch.radius,
                Pressure = touch.pressure
            };

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    InputChannels.TouchStartedEc.Raise(touchData);
                    break;
                case TouchPhase.Stationary:
                    InputChannels.TouchStationaryEc.Raise(touchData);
                    break;
                case TouchPhase.Moved:
                    InputChannels.TouchMovedEc.Raise(touchData);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                default:
                    InputChannels.TouchEndedEc.Raise(touchData);
                    break;
            }
        }

        private static bool IsPointerOverUIObject()
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}