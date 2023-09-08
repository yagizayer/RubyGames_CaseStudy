using System;
using System.Collections.Generic;
using Root.Scripts.EventHandling.BasicPassableData;

namespace Root.Scripts.EventHandling.Base
{
    public class Listener
    {
        private readonly List<Action<IPassableData>> _actions = new();

        public Listener(Action<IPassableData> action) => _actions.Add(action);

        public void OnReceive(IPassableData data) => _actions.ForEach(action => action(data));
    }
}