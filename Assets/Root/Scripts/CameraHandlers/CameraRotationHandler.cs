// CameraInput.cs

using Cinemachine;
using Root.Scripts.EventData;
using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Managers;
using UnityEngine;

namespace Root.Scripts.CameraHandlers
{
    // Cloned and edited from CinemachineTouchInputMapper.cs
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraRotationHandler : MonoBehaviour
    {
        private const float TouchSensitivityX = 10f;
        private const float TouchSensitivityY = 10f;

        private const string TouchXInputMapTo = "Mouse X";
        private const string TouchYInputMapTo = "Mouse Y";

        private float _currentDeltaX;
        private float _currentDeltaY;

        private bool _isTouching;

        private Listener _touchStartListener;
        private Listener _touchEndListener;
        private Listener _touchMoveListener;
        private Listener _touchStationaryListener;

        protected virtual void OnEnable()
        {
            _touchStartListener = new Listener(OnTouchStart);
            _touchEndListener = new Listener(OnTouchEnd);
            _touchMoveListener = new Listener(OnTouchMove);
            _touchStationaryListener = new Listener(OnTouchMove);

            InputChannels.TouchStartedEc.Subscribe(_touchStartListener);
            InputChannels.TouchEndedEc.Subscribe(_touchEndListener);
            InputChannels.TouchMovedEc.Subscribe(_touchMoveListener);
            InputChannels.TouchStationaryEc.Subscribe(_touchStationaryListener);
        }

        protected virtual void OnDisable()
        {
            InputChannels.TouchStartedEc.Unsubscribe(_touchStartListener);
            InputChannels.TouchEndedEc.Unsubscribe(_touchEndListener);
            InputChannels.TouchMovedEc.Unsubscribe(_touchMoveListener);
            InputChannels.TouchStationaryEc.Unsubscribe(_touchStationaryListener);
        }

        protected virtual void Start() => CinemachineCore.GetInputAxis = GetInputAxis;

        protected virtual float GetInputAxis(string axisName)
        {
            if (!_isTouching) return Input.GetAxis(axisName);

            return axisName switch
            {
                TouchXInputMapTo => _currentDeltaX / TouchSensitivityX,
                TouchYInputMapTo => _currentDeltaY / TouchSensitivityY,
                _ => Input.GetAxis(axisName)
            };
        }

        protected virtual void OnTouchStart(IPassableData rawData) => _isTouching = true;

        protected virtual void OnTouchEnd(IPassableData rawData)
        {
            _currentDeltaX = 0;
            _currentDeltaY = 0;
            _isTouching = false;
        }

        protected virtual void OnTouchMove(IPassableData rawData)
        {
            if (!_isTouching) return;
            var data = rawData.To<TouchData>();

            if (TargetingPad(data)) return;

            _currentDeltaX = data.DeltaPosition.x;
            _currentDeltaY = data.DeltaPosition.y;
        }

        protected virtual bool TargetingPad(TouchData data)
        {
            // create a ray that goes from the camera through the touch position
            var ray = GameManager.MainCamera.ScreenPointToRay(data.Position);

            return Physics.Raycast(ray, out _, Mathf.Infinity, GameManager.PlayAreaLayerMask);
        }
    }
}