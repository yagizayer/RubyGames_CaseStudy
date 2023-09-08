using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using UnityEngine;
using UnityEngine.Events;

namespace Root.Scripts.EventHandling.Listeners
{
    public sealed class InputChannelListener : MonoBehaviour
    {
        [SerializeField]
        private InputChannels channel;
        [SerializeField]
        private UnityEvent<IPassableData> onEventRaised;

        private Listener _listener;

        private void OnEnable()
        {
            _listener = new Listener(onEventRaised.Invoke);
            channel.Subscribe(_listener);
        }

        private void OnDisable() => channel.Unsubscribe(_listener);
        
        [ContextMenu("Debug Raise")]
        public void DebugRaise() => onEventRaised.Invoke(null);
    }
}