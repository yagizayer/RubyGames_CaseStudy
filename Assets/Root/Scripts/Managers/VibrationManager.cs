// VibrationManager.cs

using Root.Scripts.Helpers.Serialization;
using UnityEngine;

namespace Root.Scripts.Managers
{
    public sealed class VibrationManager : SingletonBase<VibrationManager>
    {
#if !UNITY_EDITOR
		private void Start() => 
            Vibration.Init();
#endif

        public void Vibrate_1()
        {
#if !UNITY_EDITOR
            Vibration.VibratePop();
#else
            Debug.Log("Vibrate_1");
#endif
        }

        public void Vibrate_2()
        {
#if !UNITY_EDITOR
            Vibration.VibratePeek();
#else
            Debug.Log("Vibrate_2");
#endif
        }

        public void Vibrate_3()
        {
#if !UNITY_EDITOR
            Vibration.VibrateNope();
#else
            Debug.Log("Vibrate_3");
#endif
        }
    }
}