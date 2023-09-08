// IEnumerableExtensions.cs


using System.Collections.Generic;

namespace Root.Scripts.Helpers.Extensions
{
    public static partial class Extensions
    {
        public static IEnumerable<T> Combine<T>(this IEnumerable<T> me, IEnumerable<T> other)
        {
            foreach (var item in me) yield return item;
            foreach (var item in other) yield return item;
        }
    }
}