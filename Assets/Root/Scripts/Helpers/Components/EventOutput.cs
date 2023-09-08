// EventOutput.cs

using Root.Scripts.EventHandling.BasicPassableData;
using UnityEngine;

namespace Root.Scripts.Helpers.Components
{
    public class EventOutput : MonoBehaviour
    {
        public void Log(string message) => Debug.Log(message);
        
        public void Log(IPassableData data) => Debug.Log(data);
    }
}