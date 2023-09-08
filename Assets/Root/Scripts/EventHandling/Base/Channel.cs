using System.Collections.Generic;
using Root.Scripts.EventHandling.BasicPassableData;

namespace Root.Scripts.EventHandling.Base
{
    public class Channel
    {
        private readonly List<Listener> _listeners = new();

        internal void Raise(IPassableData data)
        {
            var listeners = new List<Listener>(_listeners);
            foreach (var listener in listeners) listener.OnReceive(data);
        }

        internal void Subscribe(Listener listener) => _listeners.Add(listener);
        internal void Unsubscribe(Listener listener) => _listeners.Remove(listener);
    }
}