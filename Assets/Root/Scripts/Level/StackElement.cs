// StackElement.cs

using DG.Tweening;
using Root.Scripts.EventData;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Components;
using Root.Scripts.Helpers.Extensions;
using Root.Scripts.Helpers.Serialization;
using Root.Scripts.Pooling;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Root.Scripts.Level
{
    public class StackElement : DbElement
    {
        [SerializeField]
        private Rigidbody myRigidbody;

        [SerializeField]
        private TextMeshProUGUI myCounterText;

        [SerializeField]
        private float torque = 10f;

        private Transform _myTransform;
        public Pad Pad { get; private set; }
        public Pools Pool => Pad.CurrentPool;

        private static readonly Vector3 StackElementHeight = new(0, .5f, 0);
        private static readonly Vector3 StackingOffset = new(0, 1f, 0);
        public const float MergeAnimDurInSec = .25f;

        public void Start()
        {
            if (myRigidbody == null)
                myRigidbody = GetComponent<Rigidbody>();
            myRigidbody.AddTorque(Random.onUnitSphere * torque, ForceMode.Impulse);
            if (myCounterText == null)
                myCounterText = GetComponentInChildren<TextMeshProUGUI>();

            myCounterText.text = Pad.StackSize.ToString();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Pad = null;
            myCounterText.text = "";
        }


        public void AddTo(Pad pad)
        {
            if (_myTransform == null)
                _myTransform = transform;
            gameObject.SetActive(true);
            Pad = pad;
            var pos = Pad.transform.position + StackingOffset + StackElementHeight * (Pad.StackSize + 1);
            _myTransform.position = pos;
            _myTransform.SetParent(Pad.transform);
        }

        public void RemoveFrom(Pad pad)
        {
            Pad = null;
            gameObject.SetActive(false);
        }

        public void SwapTo(Pad pad)
        {
            Pad = pad;
            _myTransform.SetParent(transform);
        }

        public void AnimateMergeTo(Pad pad)
        {
            var targetPos = pad.transform.position.Modify(SnapAxis.Y, _myTransform.position.y);
            transform.DOMove(targetPos, MergeAnimDurInSec).SetEase(Ease.InOutQuad);
        }
    }
}