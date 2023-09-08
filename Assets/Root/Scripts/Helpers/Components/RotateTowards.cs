// RotateTowards.cs

using Sirenix.OdinInspector;
using UnityEngine;

namespace Root.Scripts.Helpers.Components
{
    public class RotateTowards : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private SnapAxis axis = SnapAxis.All;

        [SerializeField]
        private Vector3 rotOffset;

        private Vector3 _axisNormalized;

        private void Start()
        {
            _axisNormalized = AxisToV3().normalized;
        }

        [Button]
        public void Update()
        {
            if (target == null) target = Camera.main.transform;
            if (target == null) return;

            var targetForward = target.forward;
            var targetForwardProjected = Vector3.ProjectOnPlane(targetForward, _axisNormalized);

            transform.forward = targetForwardProjected;
            transform.eulerAngles += rotOffset;
        }

        private Vector3 AxisToV3()
        {
            var result = Vector3.zero;
            if (axis.HasFlag(SnapAxis.X)) result.x = 1;
            if (axis.HasFlag(SnapAxis.Y)) result.y = 1;
            if (axis.HasFlag(SnapAxis.Z)) result.z = 1;
            return result;
        }
    }
}