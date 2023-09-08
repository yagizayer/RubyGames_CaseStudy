// LevelLogic.cs

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Extensions;
using Root.Scripts.Pooling;
using UnityEngine;

namespace Root.Scripts.Level.Controls
{
    public class LevelLogic : MonoBehaviour
    {
        [SerializeField]
        private float levelDurationInSec = 30;

        private bool _levelEnded;
        private float _levelStartTime;
        private static List<Limit> _allLimits = new();
        private bool AllLimitsReached => _allLimits.All(limit => limit.IsReached);
        
        public static float LevelEndRatio { get; private set; }
        public static float LevelDurationInSec { get; private set; }
        public static float RemainingTime => LevelDurationInSec * (1 - LevelEndRatio);

        private void Start()
        {
            _allLimits = FindObjectsOfType<Limit>().ToList();
            LevelDurationInSec = levelDurationInSec;
            _levelStartTime = Time.time;
        }

        private void Update()
        {
            if (_levelEnded) return;
            LevelEndRatio = (Time.time - _levelStartTime) / levelDurationInSec;
            
            if (LevelEndRatio >= 1f) EndLevel(false);
        }

        public void OnLimitReached(IPassableData rawData)
        {
            if(AllLimitsReached) EndLevel(true);
        }

        private void EndLevel(bool successful)
        {
            if (_levelEnded) return;
            _levelEnded = true;

            LevelChannels.LevelEndEc.Raise(successful.ToPassable());
            LevelEndRatio = 0f;
        }

        public static Dictionary<Pools, int> GetExcessPieces()
        {
            var result = new Dictionary<Pools, int>();
            var allPieces = FindObjectsOfType<StackElement>().ToList();
            var allLimits = FindObjectsOfType<Limit>().ToList();

            foreach (var limit in allLimits)
            {
                var pool = limit.TargetPool;
                var limitValue = limit.MyLimit;
                var inPlacePiecesCount = allPieces.Count(piece =>
                    piece.Pool == pool &&
                    piece.transform.position.Near(limit.transform.position, .5f, SnapAxis.X | SnapAxis.Z));
                var excess = inPlacePiecesCount - limitValue;

                result.Add(pool, excess);
            }

            return result;
        }
    }
}