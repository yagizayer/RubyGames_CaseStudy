using System.Collections.Generic;
using Root.Scripts.EventHandling.Base;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelHandler", menuName = "Handlers/LevelHandler", order = 1)]
    public class LevelHandler : ScriptableObject
    {
        [SerializeField]
        [Title("Level Handler")]
        private int currentLevelIndex;

        [SerializeField]
        public List<LevelData> allLevels;

        private Listener _loadLevelEventListener;

        private int CurrentLevelIndex
        {
            get => GetCurrentLevelIndex() % allLevels.Count;
            set
            {
                SetCurrentLevelIndex(value);
                currentLevelIndex = value % allLevels.Count;
            }
        }

        private static LevelHandler _instance;

        private void OnEnable()
        {
            OnValidate();
            _instance = this;

            _loadLevelEventListener = new Listener(UpdateCurrentLevel);
            LevelChannels.LevelLoadEc.Subscribe(_loadLevelEventListener);
        }

        private void OnDisable()
        {
            LevelChannels.LevelLoadEc.Unsubscribe(_loadLevelEventListener);
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void OnValidate()
        {
            foreach (var level in allLevels) level.levelNumber = allLevels.IndexOf(level);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void LoadCurrentLevel() => LoadLevel(GetCurrentLevel());

        public void LoadNextLevel() => LoadLevel(allLevels[++CurrentLevelIndex % allLevels.Count]);

        public void LoadPreviousLevel() =>
            LoadLevel(allLevels[(--CurrentLevelIndex + allLevels.Count) % allLevels.Count]);

        public void LoadLevel(int levelIndex) => LoadLevel(allLevels[levelIndex]);

        // ReSharper disable Unity.PerformanceAnalysis
        public void LoadLevel(LevelData level)
        {
            if (level == null)
            {
                Debug.LogError("Level is null");
                return;
            }

            if (!allLevels.Contains(level))
            {
                Debug.LogError("Level not found");
                return;
            }

            LevelChannels.LevelLoadEc.Raise(level);
            CurrentLevelIndex = level.levelNumber;
            SetCurrentLevelIndex(CurrentLevelIndex);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void MakeCurrentLevelReady() => LevelChannels.LevelReadyEc.Raise(GetCurrentLevel());
        public void MakeCurrentLevelStart() => LevelChannels.LevelStartEc.Raise(GetCurrentLevel());

        //--------------------------------------------------------------------------------------------------------------
        public void UpdateCurrentLevel(IPassableData rawData) =>
            CurrentLevelIndex = allLevels.IndexOf((LevelData)rawData);

        public static LevelData GetCurrentLevel() => _instance.allLevels[GetCurrentLevelIndex()];

        public static int GetCurrentLevelIndex() => PlayerPrefs.GetInt("CurrentLevelIndex", 0);

        public static void SetCurrentLevelIndex(int value) => PlayerPrefs.SetInt("CurrentLevelIndex", value);

        private void ResetCurrentLevelIndex() => CurrentLevelIndex = 0;

        private void UpdateValues()
        {
#if UNITY_EDITOR
            foreach (var level in allLevels) level.UpdateValues();
#endif
        }
    }
}