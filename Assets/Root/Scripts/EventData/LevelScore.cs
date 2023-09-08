// LevelScore.cs

using System.Collections.Generic;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Pooling;
using Root.Scripts.ScriptableObjects;

namespace Root.Scripts.EventData
{
    public class LevelScore : IPassableData
    {
        public LevelData Level;
        public float Score;
        
        public Dictionary<Pools,int> ExcessPieces = new();
        public float RemainingTime;
    }
}