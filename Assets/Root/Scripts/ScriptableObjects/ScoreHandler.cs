using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects
{
    public abstract class ScoreHandler : ScriptableObject
    {
        public static float Score
        {
            get => PlayerPrefs.GetFloat("CumulativeScore", 0);
            set
            {
                PlayerPrefs.SetFloat("CumulativeScore", value);
                ScoreChangedEvent();
            }
        }

        #region Events

        private Listener _levelEndListener;
        private Listener _levelLoadListener;

        protected virtual void OnEnable()
        {
            _levelEndListener = new Listener(OnLevelEnd);
            _levelLoadListener = new Listener(OnLevelLoad);
            LevelChannels.LevelEndEc.Subscribe(_levelEndListener);
            LevelChannels.LevelLoadEc.Subscribe(_levelLoadListener);
        }

        protected virtual void OnDisable()
        {
            LevelChannels.LevelEndEc.Unsubscribe(_levelEndListener);
            LevelChannels.LevelLoadEc.Unsubscribe(_levelLoadListener);
        }

        #endregion

        #region Public Methods

        public void AddScore(float score) => Score += score * GetMultiplier();

        public void SetScore(float score) => Score = score;

        #endregion

        #region Protected Methods

        protected abstract float GetMultiplier();
        protected abstract void OnLevelEnd(IPassableData rawData);
        protected abstract void OnLevelLoad(IPassableData rawData);

        #endregion

        #region Private Methods

        private static void ScoreChangedEvent() => ScoreChannels.ScoreChangedEc.Raise(Score.ToPassable());
        
        

        #endregion
    }
}