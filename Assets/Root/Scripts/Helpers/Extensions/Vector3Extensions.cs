// Vector3Extensions.cs

using UnityEngine;

namespace Root.Scripts.Helpers.Extensions
{
    public static partial class Extensions
    {
        public static Vector3 Modify(this Vector3 me, SnapAxis targetAxis, float targetValue) =>
            targetAxis switch
            {
                SnapAxis.X => new Vector3(targetValue, me.y, me.z),
                SnapAxis.Y => new Vector3(me.x, targetValue, me.z),
                SnapAxis.Z => new Vector3(me.x, me.y, targetValue),
                _ => me
            };

        public static Vector3 ModifyAdd(this Vector3 me, SnapAxis targetAxis, float targetValue) =>
            targetAxis switch
            {
                SnapAxis.X => new Vector3(me.x + targetValue, me.y, me.z),
                SnapAxis.Y => new Vector3(me.x, me.y + targetValue, me.z),
                SnapAxis.Z => new Vector3(me.x, me.y, me.z + targetValue),
                _ => me
            };

        public static Vector3 ModifyMultiply(this Vector3 me, SnapAxis targetAxis, float targetValue) =>
            targetAxis switch
            {
                SnapAxis.X => new Vector3(me.x * targetValue, me.y, me.z),
                SnapAxis.Y => new Vector3(me.x, me.y * targetValue, me.z),
                SnapAxis.Z => new Vector3(me.x, me.y, me.z * targetValue),
                _ => me
            };

        public static bool Near(this Vector3 me, Vector3 other, float tolerance = .1f, SnapAxis axis = SnapAxis.All)
        {
            if (axis.HasFlag(SnapAxis.X) && Mathf.Abs(me.x - other.x) > tolerance) return false;
            if (axis.HasFlag(SnapAxis.Y) && Mathf.Abs(me.y - other.y) > tolerance) return false;
            if (axis.HasFlag(SnapAxis.Z) && Mathf.Abs(me.z - other.z) > tolerance) return false;
            return true;
        }
    }
}