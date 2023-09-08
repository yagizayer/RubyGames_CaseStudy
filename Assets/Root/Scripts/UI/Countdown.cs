// Countdown.cs

using System;
using System.Collections;
using System.Collections.Generic;
using Root.Scripts.Level.Controls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Root.Scripts.UI
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField]
        private Image countdownImage;

        [SerializeField]
        private TextMeshProUGUI countdownText;

        [SerializeField]
        private List<Color> gradientColors;

        private Gradient _gradient;

        private IEnumerator Start()
        {
            _gradient = new Gradient();
            var gradientColorKeys = new GradientColorKey[gradientColors.Count];
            var gradientAlphaKeys = new GradientAlphaKey[gradientColors.Count];
            for (var i = 0; i < gradientColors.Count; i++)
            {
                gradientColorKeys[i].color = gradientColors[i];
                gradientColorKeys[i].time = i / (gradientColors.Count - 1f);
                gradientAlphaKeys[i].alpha = gradientColors[i].a;
                gradientAlphaKeys[i].time = i / (gradientColors.Count - 1f);
            }

            _gradient.SetKeys(gradientColorKeys, gradientAlphaKeys);

            if (countdownText == null) yield break;
            var reduceInterval = new WaitForSeconds(1);
            while (LevelLogic.LevelEndRatio <= 1f)
            {
                if(!enabled) yield break;
                countdownText.text = Mathf.FloorToInt(LevelLogic.RemainingTime).ToString();
                yield return reduceInterval;
            }
        }

        private void Update()
        {
            countdownImage.fillAmount = 1 - LevelLogic.LevelEndRatio;
            countdownImage.color = _gradient.Evaluate(LevelLogic.LevelEndRatio);
        }
    }
}