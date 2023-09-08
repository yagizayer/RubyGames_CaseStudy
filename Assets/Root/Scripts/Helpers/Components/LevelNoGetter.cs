// LevelNoGetter.cs

using Root.Scripts.ScriptableObjects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Root.Scripts.Helpers.Components
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LevelNoGetter : MonoBehaviour
    {
        [HideLabel]
        [SerializeField]
        [HorizontalGroup]
        [OnValueChanged(nameof(UpdateLevelText))]
        private string prefix = "Level ";

        [ReadOnly]
        [HideLabel]
        [SerializeField]
        [HorizontalGroup]
        private string level = "";

        [HideLabel]
        [SerializeField]
        [HorizontalGroup]
        private string suffix = "";

        [SerializeField]
        private int levelOffset = 1;

        private TextMeshProUGUI _text;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            UpdateLevelText();
        }

        private void OnValidate()
        {
            UpdateLevelText();
        }

        private void UpdateLevelText()
        {
            if (_text == null) _text = GetComponent<TextMeshProUGUI>();
            level = (LevelHandler.GetCurrentLevelIndex() + levelOffset).ToString("0");
            _text.text = prefix + level + suffix;
        }
    }
}