// Pad.cs

using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Root.Scripts.EventData;
using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Components;
using Root.Scripts.Helpers.Extensions;
using Root.Scripts.Helpers.Serialization;
using Root.Scripts.Managers;
using Root.Scripts.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Root.Scripts.Level
{
    public class Pad : DbElement
    {
        private readonly List<StackElement> _stack = new();

        [SerializeField]
        private Pools currentPool = Pools.Null;

        [field: SerializeField, ReadOnly] public List<Pad> Neighbours { get; private set; } = new();

        private bool _mergingInProgress;
        private bool _isLocked;
        public int StackSize => _stack.Count;
        public Pools CurrentPool => currentPool;
        public bool IsLocked => _isLocked;


        private void Start() => SetNeighbours();

        public void OnTappedPad(IPassableData rawData)
        {
            var data = rawData.To<TappedPadData>();
            if (data.Pad != this) return;
            if (_isLocked) return;
            if (!ValidateTap()) return;


            var newStackElement = CreateStackElement(Pools.StackElement1.ToPoolName());
            currentPool = Pools.StackElement1;

            GameplayChannels.StackSizeChangedEc.Raise(new StackSizeChangedData
            {
                NewStackSize = _stack.Count,
                Pad = this,
                NewElement = newStackElement
            });
        }

        public void OnSwipedPad(IPassableData rawData)
        {
            var data = rawData.To<SwipedPadData>();
            if (data.EndPad != this) return;
            if (_isLocked) return;
            if (data.StartPad._isLocked) return;
            if (!ValidateMerge(data.StartPad)) return;

            HandleMerge(data.StartPad);
        }

        public void OnLimitReached(IPassableData rawData)
        {
            var data = rawData.To<LimitReachedData>();
            if (data.TargetPool != currentPool) return;
            if (data.Pad != this) return;
            _isLocked = true;
        }

        #region Private Methods

        private void HandleMerge(Pad startPad)
        {
            if (currentPool == Pools.Null) SwapStacks(startPad);
            else MergeStacks(startPad);
        }

        private void SwapStacks(Pad startPad)
        {
            _mergingInProgress = true;
            startPad._mergingInProgress = true;

            foreach (var element in startPad._stack) element.AnimateMergeTo(this);

            DOTween.Sequence()
                .AppendInterval(StackElement.MergeAnimDurInSec)
                .AppendCallback(OnAnimationComplete);


            return;

            void OnAnimationComplete()
            {
                foreach (var element in startPad._stack)
                {
                    element.SwapTo(this);
                    _stack.Add(element);
                }

                startPad._stack.Clear();
                currentPool = startPad.currentPool;
                startPad.currentPool = Pools.Null;

                _mergingInProgress = false;
                startPad._mergingInProgress = false;

                var swapData = new StackSwapCompletedData
                {
                    NewStackSize = _stack.Count,
                    Pad = this,
                    SwappedElements = new List<StackElement>(_stack)
                };

                GameplayChannels.StackSwapCompletedEc.Raise(swapData);
            }
        }

        private void MergeStacks(Pad startPad)
        {
            _mergingInProgress = true;
            startPad._mergingInProgress = true;

            var newStackSize = GetNewStackSize(startPad);
            var objectsToReturn = _stack.Combine(startPad._stack).ToList();
            var returnPool = startPad.currentPool;
            var newStackPool = startPad.currentPool.NextPool();

            foreach (var element in objectsToReturn) element.AnimateMergeTo(this);
            DOTween.Sequence()
                .AppendInterval(StackElement.MergeAnimDurInSec)
                .AppendCallback(OnAnimationComplete);


            return;

            void OnAnimationComplete()
            {
                foreach (var element in objectsToReturn)
                {
                    element.RemoveFrom(this);
                    PoolManager.Return(returnPool.ToPoolName(), element.gameObject);
                }

                _stack.Clear();
                startPad._stack.Clear();

                for (var elementNo = 0; elementNo < newStackSize; elementNo++)
                    CreateStackElement(newStackPool.ToPoolName());

                currentPool = newStackPool;
                startPad.currentPool = Pools.Null;

                _mergingInProgress = false;
                startPad._mergingInProgress = false;

                var mergeData = new StackMergeCompletedData
                {
                    NewStackSize = _stack.Count,
                    Pad = this,
                    NewElements = new List<StackElement>(_stack),
                    RemovedElements = objectsToReturn,
                    Key = "Merge",
                    Pitch01 = _stack.Count.Remap(0, 5, 0, 1)
                };

                GameplayChannels.StackMergeCompletedEc.Raise(mergeData);
            }
        }

        private bool ValidateMerge(Pad startPad)
        {
            if (startPad == null) return false;
            if (startPad == this) return false;
            if (_mergingInProgress) return false;
            if (currentPool == Pools.Null)
            {
                // if my stack is empty
                var otherNotEmpty = startPad.currentPool != Pools.Null;

                return otherNotEmpty;
            }
            else
            {
                // if my stack is not empty
                var samePool = startPad.currentPool == currentPool;
                var otherNotEmpty = startPad.currentPool != Pools.Null;
                var otherNotMax = startPad.currentPool != Pools.StackElement4;

                return samePool && otherNotEmpty && otherNotMax;
            }
        }

        private bool ValidateTap()
        {
            if (_mergingInProgress) return false;
            if (currentPool == Pools.Null) return true;
            if (currentPool == Pools.StackElement1) return true;
            return false;
        }

        private int GetNewStackSize(Pad startPad)
        {
            var otherStackSize = startPad.StackSize;
            // if my stack is empty, I take the other stack
            // if my stack is not empty, take average of both stacks
            var newStackSize = Mathf.FloorToInt((StackSize + otherStackSize) / 2f);
            newStackSize = StackSize == 0 ? otherStackSize : newStackSize;
            return newStackSize;
        }

        private StackElement CreateStackElement(string poolName)
        {
            var newStackElement = PoolManager.Get<StackElement>(poolName);
            newStackElement.AddTo(this);
            _stack.Add(newStackElement);
            return newStackElement;
        }

        private void SetNeighbours()
        {
            var myTransform = transform;
            var myPosition = myTransform.position;
            var raycastPositions = new[]
            {
                myPosition + Vector3.up + Vector3.forward * 5,
                myPosition + Vector3.up + Vector3.back * 5,
                myPosition + Vector3.up + Vector3.left * 5,
                myPosition + Vector3.up + Vector3.right * 5
            };
            var raycastDirection = Vector3.down;

            foreach (var raycastPosition in raycastPositions)
            {
                var ray = new Ray(raycastPosition, raycastDirection);
                if (!Physics.Raycast(ray, out var hit, 100, layerMask: GameManager.PlayAreaLayerMask)) continue;
                if (!ReferenceDb.TryGetValue(hit.transform, out Pad hitPad)) continue;

                Neighbours.Add(hitPad);
            }
        }

        #endregion
    }
}