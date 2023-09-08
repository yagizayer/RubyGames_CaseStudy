// TapSwipeHandler.cs

using System;
using Root.Scripts.EventData;
using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Extensions;
using Root.Scripts.Managers;
using UnityEngine;

namespace Root.Scripts.Level.Controls
{
    public class TapSwipeHandler : MonoBehaviour
    {
        private const float MaxTapDistance = 50f;
        private static Pad _swipeStartPad;
        private static Vector2 _tapStartPosition;

        public void OnTouchStart(IPassableData rawData)
        {
            var data = rawData.To<TouchData>();
            if (!GetTargetPad(data, out var pad)) return;
            _swipeStartPad = pad;
            _tapStartPosition = data.Position;
        }

        public void OnTouchEnd(IPassableData rawData)
        {
            var data = rawData.To<TouchData>();

            if (ValidateSwipe(data, out var swipedPadData))
                GameplayChannels.SwipedPadEc.Raise(swipedPadData);
            else if (ValidateTap(data, out var tappedPadData))
                GameplayChannels.TappedPadEc.Raise(tappedPadData);
        }

        private static bool ValidateTap(TouchData data, out TappedPadData eventData)
        {
            eventData = new TappedPadData();

            if ((data.Position - _tapStartPosition).sqrMagnitude > MaxTapDistance * MaxTapDistance)
                return false;
            if (!GetTargetPad(data, out var pad))
                return false;
            if (pad.IsLocked)
                return false;

            eventData.Pad = pad;
            eventData.Key = "Tap";
            eventData.Pitch01 = pad.StackSize.Remap(0, 5, 0, 1);
            return true;
        }

        private static bool ValidateSwipe(TouchData data, out SwipedPadData eventData)
        {
            eventData = new SwipedPadData();

            if (_swipeStartPad == null)
                return false;
            if (!GetTargetPad(data, out var pad))
                return false;
            if (pad == _swipeStartPad)
                return false;
            if (!_swipeStartPad.Neighbours.Contains(pad))
                return false;
            if (_swipeStartPad.IsLocked || pad.IsLocked)
                return false;

            eventData.StartPad = _swipeStartPad;
            eventData.EndPad = pad;
            eventData.SwipeDirectionInWorld = pad.transform.position - _swipeStartPad.transform.position;
            eventData.Key = "Swipe";
            eventData.Pitch01 = (pad.StackSize + _swipeStartPad.StackSize).Remap(0, 5, 0, 1);
            return true;
        }

        private static bool GetTargetPad(TouchData data, out Pad pad)
        {
            pad = null;
            var ray = GameManager.MainCamera.ScreenPointToRay(data.Position);
            if (!Physics.Raycast(ray, out var hit, 100, GameManager.PlayAreaLayerMask)) return false;
            if (ReferenceDb.TryGetValue(hit.transform, out StackElement stackElement))
            {
                // target is a stack element
                pad = stackElement.Pad;
            }
            else if (ReferenceDb.TryGetValue(hit.transform, out pad))
            {
                // target is a pad
            }
            else return false;

            return true;
        }
    }
}