using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Interfaces;
using Root.Scripts.Helpers.Serialization;
using Root.Scripts.Pooling;
using Root.Scripts.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Root.Scripts.EventHandling.Base;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Root.Scripts.Managers
{
    public class GameManager : SingletonBase<GameManager>
    {
        #region References

        [SerializeField]
        [TabGroup("LevelCreator")]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        private LevelCreationHandler levelCreationHandler;

        [SerializeField]
        [TabGroup("LevelHandler")]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        private LevelHandler levelHandler;

        [SerializeField]
        [TabGroup("ScoreHandler")]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        private CustomScoreHandler scoreHandler;

        [SerializeField]
        [TabGroup("PoolReferences")]
        [Pool(Pools.StackElement1, 300)]
        public AssetReferenceGameObject stackElement1;

        [SerializeField]
        [TabGroup("PoolReferences")]
        [Pool(Pools.StackElement2, 300)]
        public AssetReferenceGameObject stackElement2;

        [SerializeField]
        [TabGroup("PoolReferences")]
        [Pool(Pools.StackElement3, 200)]
        public AssetReferenceGameObject stackElement3;

        [SerializeField]
        [TabGroup("PoolReferences")]
        [Pool(Pools.StackElement4, 100)]
        public AssetReferenceGameObject stackElement4;

        #endregion

        #region Buttons

        [ButtonGroup("LevelControls")]
        private void LoadPreviousLevel()
        {
            levelHandler.LoadPreviousLevel();
            OnLevelLoad(LevelHandler.GetCurrentLevel());
        }

        [ButtonGroup("LevelControls")]
        private void LoadCurrentLevel()
        {
            levelHandler.LoadCurrentLevel();
            OnLevelLoad(LevelHandler.GetCurrentLevel());
        }

        [ButtonGroup("LevelControls")]
        private void LoadNextLevel()
        {
            levelHandler.LoadNextLevel();
            OnLevelLoad(LevelHandler.GetCurrentLevel());
        }

        #endregion

        public static Camera MainCamera { get; private set; }
        public static LayerMask PlayAreaLayerMask { get; private set; }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            StartCoroutine(CheckForDelayers_CO());

            PlayAreaLayerMask = LayerMask.GetMask("PlayArea");
            PoolManager.Initialize();
            if (Pool.AllPools != null)
                DontDestroyOnLoad(Pool.AllPools);
        }


        //--------------------------------------------------------------------------------------------------------------
        public void OnLevelLoad(IPassableData rawData)
        {
            var data = rawData.To<LevelData>();
            UnloadLevels(onUnloadComplete: () => { LoadLevel(data); });
        }

        public void OnLevelReady(IPassableData rawData)
        {
            MainCamera = Camera.main;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void LoadLevel(LevelData data)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                data.levelReference.LoadSceneAsync(LoadSceneMode.Additive).Completed += _ =>
                    DOTween.Sequence().AppendInterval(0).AppendCallback(() => LevelChannels.LevelReadyEc.Raise(data));
            }
            else
            {
                var path = AssetDatabase.GUIDToAssetPath(data.levelReference.AssetGUID);
                SceneManager.GetSceneByPath(path);
                EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            }
#else
            data.levelReference.LoadSceneAsync(LoadSceneMode.Additive).Completed += _ =>
            {
                DOTween.Sequence().AppendInterval(0).AppendCallback(() => LevelChannels.LevelReadyEc.Raise(data));
            };
#endif
        }

        public static void UnloadLevels(Action onUnloadComplete)
        {
            var activeLevelScenes = new List<Scene>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
                if (SceneManager.GetSceneAt(i).name.StartsWith("Level_"))
                    activeLevelScenes.Add(SceneManager.GetSceneAt(i));
#if UNITY_EDITOR
            for (var sceneNo = 0; sceneNo < activeLevelScenes.Count; sceneNo++)
            {
                var scene = activeLevelScenes[sceneNo];
                if (Application.isPlaying)
                {
                    var temp = sceneNo;
                    SceneManager.UnloadSceneAsync(scene).completed += _ =>
                    {
                        if (temp == 0)
                            onUnloadComplete?.Invoke();
                    };
                }
                else
                {
                    EditorSceneManager.CloseScene(scene, true);
                    if (sceneNo == 0)
                        onUnloadComplete?.Invoke();
                }
            }

            if (activeLevelScenes.Count == 0) onUnloadComplete?.Invoke();
#else
            for (var sceneNo = 0; sceneNo < activeLevelScenes.Count; sceneNo++)
            {
                var scene = activeLevelScenes[sceneNo];
                var temp = sceneNo;
                SceneManager.UnloadSceneAsync(scene).completed += _ =>
                {
                    if (temp == activeLevelScenes.Count - 1)
                        onUnloadComplete?.Invoke();
                };
            }

            if (activeLevelScenes.Count == 0) onUnloadComplete?.Invoke();
#endif
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CheckForDelayers_CO()
        {
            var loadDelayers = FindObjectsOfType<MonoBehaviour>().OfType<ILoadDelayer>().ToList();
            if (loadDelayers.Count == 0)
            {
                levelHandler.LoadCurrentLevel();
                yield break;
            }

            var checkInterval = new WaitForSeconds(.5f);
            while (true)
            {
                // wait for all Delayers to finish then load saved level
                yield return checkInterval;
                if (!loadDelayers.All(x => x.DelayCondition)) continue;
                levelHandler.LoadCurrentLevel();
                break;
            }
        }
    }
}