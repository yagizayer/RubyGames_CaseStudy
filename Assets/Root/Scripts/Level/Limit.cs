// Limit.cs

using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Root.Scripts.EventData;
using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Extensions;
using Root.Scripts.Level.Controls;
using Root.Scripts.Managers;
using Root.Scripts.Pooling;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Root.Scripts.Level
{
    public class Limit : MonoBehaviour
    {
        [SerializeField]
        private Pools targetPool = Pools.StackElement1;

        [SerializeField]
        private BoxCollider myCollider;

        [SerializeField]
        private TextMeshProUGUI heightText;

        [SerializeField]
        private Animator animator;


        [ReadOnly]
        [SerializeField]
        [Tooltip("Use Scale Y to set the limit")]
        private int myLimit;

        private bool _isReached;
        private static readonly int Warn = Animator.StringToHash("Warn");

        public int MyLimit => myLimit;
        public Pools TargetPool => targetPool;
        public bool IsReached => _isReached;

        private void Start()
        {
            var myScale = transform.parent.localScale;
            heightText.text = myScale.y + "";
            myLimit = (int)myScale.y;
        }

        private void Update()
        {
            if (_isReached)
            {
                animator.SetBool(Warn, false);
                return;
            }

            if (LevelLogic.RemainingTime > 6) return;

            animator.SetBool(Warn, true);
        }

        private void OnTriggerEnter(Collider other)
        {
            var bounds = myCollider.bounds;
            var colliders = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity);
            foreach (var col in colliders)
            {
                if (!ReferenceDb.TryGetValue(col.transform, out StackElement se)) continue;
                if (se.Pool != targetPool) continue;

                _isReached = true;
            }

            if (!_isReached) return;

            DOTween.Sequence().AppendInterval(1).AppendCallback(() =>
            {
                if (!_isReached) return;

                var pad = GetPad();
                if (pad.IsLocked) return;

                var eventData = new LimitReachedData
                {
                    TargetPool = targetPool,
                    Pad = GetPad(),
                    Key = "LimitReached",
                    Pitch01 = pad.StackSize.Remap(0, myLimit, 0f, 1),
                };
                GameplayChannels.LimitReachedEc.Raise(eventData);
            });
        }

        private Pad GetPad()
        {
            var allPads = FindObjectsOfType<Pad>().ToList();

            var potentialPads =
                allPads.Where(pad =>
                    pad.CurrentPool == targetPool &&
                    pad.transform.position.Near(transform.position, .5f, SnapAxis.X | SnapAxis.Z));

            return potentialPads.First();
        }
    }
}