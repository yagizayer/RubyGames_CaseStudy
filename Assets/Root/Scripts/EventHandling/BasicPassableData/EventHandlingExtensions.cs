// EventHandlingExtensions.cs

namespace Root.Scripts.EventHandling.BasicPassableData
{
    public static class EventHandlingExtensions
    {
        public static bool TryGetValue<T>(this IPassableData me, out T data)
        {
            data = default;
            if (me is not T outData) return false;
            data = outData;
            return true;
        }

        public static IPassableData ToPassable<T>(this T me) => new GenericPassableData<T>(me);
        public static T To<T>(this IPassableData me) => (T)me;
    }
}