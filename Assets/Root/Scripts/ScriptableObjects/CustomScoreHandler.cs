using System.Collections.Generic;
using System.Linq;
using Root.Scripts.EventData;
using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Serialization;
using Root.Scripts.Level.Controls;
using Root.Scripts.Pooling;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CustomScoreHandler", menuName = "Handlers/CustomScoreHandler")]
    public sealed class CustomScoreHandler : ScoreHandler
    {
        [SerializeField]
        private SerializableDictionary<Pools, int> pieceMultipliers = new();

        protected override void OnLevelLoad(IPassableData rawData)
        {
        }

        protected override float GetMultiplier() => 1;


        protected override void OnLevelEnd(IPassableData rawData)
        {
            var data = rawData.To<GenericPassableData<bool>>();
            var excessPieces = LevelLogic.GetExcessPieces();
            var scoreOfThisLevel = CalculateScore(excessPieces);

            var levelScore = new LevelScore
            {
                Level = LevelHandler.GetCurrentLevel(),
                Score = data.Value ? scoreOfThisLevel : 0,
                ExcessPieces = excessPieces,
                RemainingTime = LevelLogic.RemainingTime
            };

            AddScore(levelScore.Score);
            if (data.Value)
                LevelChannels.LevelSuccessfulEc.Raise(levelScore);
            else
                LevelChannels.LevelFailedEc.Raise(levelScore);
        }

        private int CalculateScore(Dictionary<Pools, int> excessPieces)
        {
            var excessScoreMultiplier = excessPieces.Sum(
                excessPiece =>
                    pieceMultipliers[excessPiece.Key] * excessPiece.Value
            );
            var baseScore = Mathf.CeilToInt(LevelLogic.RemainingTime);
            var excessScore = excessScoreMultiplier * baseScore;
            var scoreOfThisLevel = baseScore + excessScore;
            return Mathf.Max(0, scoreOfThisLevel);
        }
    }
}