// CameraPositionHandlers.cs

using System;
using Cinemachine;
using Root.Scripts.EventData;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Level;
using UnityEngine;

namespace Root.Scripts.CameraHandlers
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraPositionHandler : MonoBehaviour
    {
        private CinemachineTargetGroup _camTargetGroup;
        private CinemachineVirtualCamera _cam;

        private CinemachineTransposer _camTransposer;
        private float _yOffset;

        private void Start() => Initialize();

        public void OnStackSizeChanged(IPassableData rawData)
        {
            var data = rawData.To<StackSizeChangedData>();

            _camTargetGroup.AddMember(data.NewElement.transform, 1, 0);
            _camTransposer.m_FollowOffset.y = _camTargetGroup.transform.position.y * 2 + _yOffset;
        }

        public void OnStackMergeCompleted(IPassableData rawData)
        {
            var data = rawData.To<StackMergeCompletedData>();

            foreach (var removedElement in data.RemovedElements)
                _camTargetGroup.RemoveMember(removedElement.transform);
            foreach (var newElement in data.NewElements)
                _camTargetGroup.AddMember(newElement.transform, 1, 0);

            _camTransposer.m_FollowOffset.y = _camTargetGroup.transform.position.y * 2 + _yOffset;
        }

        private void Initialize()
        {
            var pads = FindObjectsOfType<Pad>();
            _cam = GetComponent<CinemachineVirtualCamera>();
            _camTargetGroup = _cam.LookAt.GetComponent<CinemachineTargetGroup>();
            _camTargetGroup.m_Targets = Array.Empty<CinemachineTargetGroup.Target>();
            foreach (var pad in pads) _camTargetGroup.AddMember(pad.transform, 1, 0);

            if (_camTransposer == null)
                _camTransposer = _cam.GetCinemachineComponent<CinemachineTransposer>();
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_yOffset == default)
                _yOffset = _camTransposer.m_FollowOffset.y;
        }
    }
}