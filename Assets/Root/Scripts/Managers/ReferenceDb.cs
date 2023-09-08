using System.Collections.Generic;
using UnityEngine;

namespace Root.Scripts.Managers
{
    public static class ReferenceDb
    {
        private static readonly Dictionary<Transform, List<object>> Database = new();

        public static void Set<T>(Transform t, T value) where T : class
        {
            if (Database.ContainsKey(t))
                Database[t].Add(value);
            else
                Database.Add(t, new List<object> { value });
        }

        public static T Get<T>(Transform t) where T : class
        {
            if (!Database.TryGetValue(t, out var values)) return null;
            return values.Find(result => result is T) as T;
        }

        public static void Remove(Transform t)
        {
            if (Database.ContainsKey(t))
                Database.Remove(t);
        }

        public static void Clear() => Database.Clear();

        public static bool Contains(Transform t) => Database.ContainsKey(t);

        public static bool TryGetValue<T>(Transform t, out T value) where T : class
        {
            value = null;
            if (!Database.TryGetValue(t, out var values)) return false;

            value = values.Find(result => result is T) as T;
            return value != null;
        }
    }
}