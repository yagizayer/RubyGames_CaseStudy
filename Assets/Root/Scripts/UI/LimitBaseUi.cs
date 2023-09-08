// LimitBaseUi.cs

using Root.Scripts.EventData;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Pooling;
using UnityEngine;

namespace Root.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class LimitBaseUi : MonoBehaviour
    {
        [SerializeField]
        private Pools targetPool = Pools.StackElement1;

        [SerializeField]
        private Animator animator;

        private static readonly int Check = Animator.StringToHash("Check");
        
        public void OnLimitReached(IPassableData rawData)
        {
            var data = rawData.To<LimitReachedData>();
            if (data.TargetPool != targetPool) return;
            
            animator.Play(Check);
        }
    }
}