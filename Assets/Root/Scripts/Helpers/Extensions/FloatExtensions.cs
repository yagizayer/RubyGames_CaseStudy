// FloatExtensions.cs

using UnityEngine;

namespace Root.Scripts.Helpers.Extensions
{
    public static partial class Extensions
    {
        public static float Remap(this float value, float from1, float to1, float from2, float to2) =>
            (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        
        public static float Remap(this int value, float from1, float to1, float from2, float to2) =>
            (value - from1) / (to1 - from1) * (to2 - from2) + from2;

        public static float Remap(this float value, Vector2 range, Vector2 targetRange) =>
            (value - range.x) / (range.y - range.x) * (targetRange.y - targetRange.x) + targetRange.x;

        public static float Remap(this int value, Vector2 range, Vector2 targetRange) =>
            (value - range.x) / (range.y - range.x) * (targetRange.y - targetRange.x) + targetRange.x;
    }
}