using System;
using Root.Scripts.EventData;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Serialization;
using Root.Scripts.Level;
using Root.Scripts.Pooling;
using Root.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Root.Scripts.UI
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField]
        private RectTransform screenParent;

        [SerializeField]
        private Animator screenAnimator;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        [SerializeField]
        private TextMeshProUGUI totalScoreText;

        [SerializeField]
        private TextMeshProUGUI remainingTimeText;

        [SerializeField]
        private SerializableDictionary<Pools, TextMeshProUGUI> excessPiecesTexts = new()
        {
            { Pools.StackElement1, null },
            { Pools.StackElement2, null },
            { Pools.StackElement3, null },
            { Pools.StackElement4, null },
        };

        public void OnLevelSuccessfulOrUnsuccessful(IPassableData rawData)
        {
            var data = rawData.To<LevelScore>();
            var activeLimits = FindObjectsOfType<Limit>();
            foreach (var limit in activeLimits)
                // i know this is dirty but i don't want to reference the limit objects by hand
                excessPiecesTexts[limit.TargetPool].transform.parent.parent.parent.gameObject.SetActive(true);

            scoreText.text = data.Score + "";
            totalScoreText.text = ScoreHandler.Score + "";
            remainingTimeText.text = data.RemainingTime.ToString("0");

            foreach (var excessPiece in data.ExcessPieces)
                excessPiecesTexts[excessPiece.Key].text = excessPiece.Value + "";
            Show();
        }

        public void Show()
        {
            if (screenAnimator != null) screenAnimator.Play("Show");
            else screenParent.gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (screenAnimator != null) screenAnimator.Play("Hide");
            else screenParent.gameObject.SetActive(false);
        }
    }
}