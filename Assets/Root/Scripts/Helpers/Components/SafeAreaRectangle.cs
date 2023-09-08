using UnityEngine;

namespace Root.Scripts.Helpers.Components
{
    public class SafeAreaRectangle : MonoBehaviour
    {
        private Canvas _canvas;

        [ContextMenu("Update")]
        public void Start()
        {
            var safeArea = Screen.safeArea;
            _canvas = GetComponentInParent<Canvas>();
            var safeAreaTransform = transform as RectTransform;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            var pixelRect = _canvas.pixelRect;

            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;
            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;

            if (safeAreaTransform == null) return;

            safeAreaTransform.sizeDelta = Vector2.zero;
            safeAreaTransform.anchoredPosition = Vector2.zero;
            safeAreaTransform.anchorMin = anchorMin;
            safeAreaTransform.anchorMax = anchorMax;
        }
    }
}