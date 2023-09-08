using DG.Tweening;
using Root.Scripts.EventHandling.Base;
using Root.Scripts.Helpers.Extensions;
using Root.Scripts.Managers;
using Root.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
#endif

namespace Root.Editor
{
    public static class CustomShortcuts
    {
#if UNITY_EDITOR

        private static string _levelNoAsString = "";

        private const string LevelHandlerPath =
            "Assets/Root/ScriptableObjects/LevelHandler.asset";

        private const string LevelCreatıonHandlerPath =
            "Assets/Root/ScriptableObjects/LevelCreator.asset";

        private const string BaseScenePath =
            "Assets/Root/Scenes/Base.unity";

        /*
         * Level Creation
         * New Level    = Shift + N
         * NextLevel    = Shift + T
         * ReloadLevel  = Shift + R
         * PrevLevel    = Shift + E
         * Open Level X = Shift + AplhaXX // 00 - 99
         *
         * Project Window Bookmarks
         * Goto Prefabs = Shift + G
         * Goto Audios  = Shift + F
         * Goto Sprites = Shift + D 
         * Goto Models  = Shift + S
         *
         * Time Scaling
         * Speed up     = Shift + KeyPadPlus
         * Speed down   = Shift + KeyPadMinus
         */

        private static LevelHandler _levelHandler;
        private static LevelCreationHandler _levelCreationHandler;

        #region Level Shortcuts

        [Shortcut("CustomShortcuts/NewLevel", KeyCode.N, ShortcutModifiers.Shift)]
        private static void NewLevel()
        {
            SetStaticReferences();
            _levelCreationHandler.CreateLevel();
        }

        [Shortcut("CustomShortcuts/NextLevel", KeyCode.T, ShortcutModifiers.Shift)]
        private static void NextLevel()
        {
            SetStaticReferences();
            LoadLevelFromCurrentLevel(1);
        }

        [Shortcut("CustomShortcuts/ReloadLevel", KeyCode.R, ShortcutModifiers.Shift)]
        private static void ReloadLevel()
        {
            SetStaticReferences();
            LoadLevelFromCurrentLevel(0);
        }

        [Shortcut("CustomShortcuts/PrevLevel", KeyCode.E, ShortcutModifiers.Shift)]
        private static void PrevLevel()
        {
            SetStaticReferences();
            LoadLevelFromCurrentLevel(-1);
        }

        [Shortcut("CustomShortcuts/BaseScene", KeyCode.B, ShortcutModifiers.Shift)]
        private static void InitScene()
        {
            if (Application.isPlaying) return;
            SetStaticReferences();
            EditorSceneManager.OpenScene(BaseScenePath);
            LoadLevelFromCurrentLevel(0, true);
        }

        [Shortcut("CustomShortcuts/CurrentLevel", KeyCode.C, ShortcutModifiers.Shift)]
        private static void CurrentLevel()
        {
            if (Application.isPlaying) return;
            SetStaticReferences();
            LoadLevelFromCurrentLevel(0, true);
        }

        #region Load Level By Number

        [ClutchShortcut("CustomShortcuts/LevelNumberReset", KeyCode.BackQuote, ShortcutModifiers.Shift)]
        private static void LevelNumberReset(ShortcutArguments args) => _levelNoAsString = "";


        [ClutchShortcut("CustomShortcuts/LevelNumber0", KeyCode.Alpha0, ShortcutModifiers.Shift)]
        private static void LevelNumber0(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "0";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        [ClutchShortcut("CustomShortcuts/LevelNumber1", KeyCode.Alpha1, ShortcutModifiers.Shift)]
        private static void LevelNumber1(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "1";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        [ClutchShortcut("CustomShortcuts/LevelNumber2", KeyCode.Alpha2, ShortcutModifiers.Shift)]
        private static void LevelNumber2(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "2";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        [ClutchShortcut("CustomShortcuts/LevelNumber3", KeyCode.Alpha3, ShortcutModifiers.Shift)]
        private static void LevelNumber3(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "3";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        [ClutchShortcut("CustomShortcuts/LevelNumber4", KeyCode.Alpha4, ShortcutModifiers.Shift)]
        private static void LevelNumber4(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "4";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        [ClutchShortcut("CustomShortcuts/LevelNumber5", KeyCode.Alpha5, ShortcutModifiers.Shift)]
        private static void LevelNumber5(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "5";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        [ClutchShortcut("CustomShortcuts/LevelNumber6", KeyCode.Alpha6, ShortcutModifiers.Shift)]
        private static void LevelNumber6(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "6";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        [ClutchShortcut("CustomShortcuts/LevelNumber7", KeyCode.Alpha7, ShortcutModifiers.Shift)]
        private static void LevelNumber7(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "7";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        [ClutchShortcut("CustomShortcuts/LevelNumber8", KeyCode.Alpha8, ShortcutModifiers.Shift)]
        private static void LevelNumber8(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "8";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        [ClutchShortcut("CustomShortcuts/LevelNumber9", KeyCode.Alpha9, ShortcutModifiers.Shift)]
        private static void LevelNumber9(ShortcutArguments args)
        {
            switch (args.stage)
            {
                case ShortcutStage.Begin:
                    _levelNoAsString += "9";
                    break;
                case ShortcutStage.End when _levelNoAsString.Length == 2:
                    LoadLevelFromLevelNo();
                    break;
                default: break;
            }
        }

        #endregion

        #region Time Scaling

        [Shortcut("CustomShortcuts/SpeedUp", KeyCode.KeypadPlus, ShortcutModifiers.Shift)]
        private static void SpeedUp()
        {
            Time.timeScale += 0.25f;
        }

        [Shortcut("CustomShortcuts/SpeedDown", KeyCode.KeypadMinus, ShortcutModifiers.Shift)]
        private static void SpeedDown()
        {
            Time.timeScale -= 0.25f;
        }

        #endregion

        #endregion

        #region private methods

        private static void SetStaticReferences()
        {
            _levelHandler = AssetDatabase.LoadAssetAtPath<LevelHandler>(LevelHandlerPath);
            _levelCreationHandler = AssetDatabase.LoadAssetAtPath<LevelCreationHandler>(LevelCreatıonHandlerPath);
        }

        private static void LoadLevelFromLevelNo()
        {
            SetStaticReferences();
            var targetLevelNo = int.Parse(_levelNoAsString) - 1;
            if (_levelHandler.allLevels.Count <= targetLevelNo)
            {
                UnityEngine.Debug.LogError($"Level number {_levelNoAsString} does not exist!");
                _levelNoAsString = "";
                return;
            }

            var data = _levelHandler.allLevels[int.Parse(_levelNoAsString) - 1];
            GameManager.UnloadLevels(onUnloadComplete: () =>
            {
                if (Application.isPlaying)
                {
                    data.levelReference.LoadSceneAsync(LoadSceneMode.Additive).Completed += _ =>
                    {
                        DOTween.Sequence().AppendInterval(0)
                            .AppendCallback(() => LevelChannels.LevelReadyEc.Raise(data));
                    };
                }
                else
                {
                    var path = AssetDatabase.GUIDToAssetPath(data.levelReference.AssetGUID);
                    SceneManager.GetSceneByPath(path);
                    EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                }

                LevelHandler.SetCurrentLevelIndex(targetLevelNo);
            });
            _levelNoAsString = "";
        }

        private static void LoadLevelFromCurrentLevel(int levelOffset, bool additive = false)
        {
            SetStaticReferences();
            var targetLevelNo = (
                                    LevelHandler.GetCurrentLevelIndex()
                                    + levelOffset
                                    + _levelHandler.allLevels.Count
                                )
                                % _levelHandler.allLevels.Count;
            var data = _levelHandler.allLevels[targetLevelNo];
            GameManager.UnloadLevels(onUnloadComplete: () =>
            {
                if (Application.isPlaying)
                {
                    var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
                    data.levelReference.LoadSceneAsync(mode).Completed += _ =>
                    {
                        DOTween.Sequence().AppendInterval(0)
                            .AppendCallback(() => LevelChannels.LevelReadyEc.Raise(data));
                    };
                }
                else
                {
                    var mode = additive ? OpenSceneMode.Additive : OpenSceneMode.Single;
                    var path = AssetDatabase.GUIDToAssetPath(data.levelReference.AssetGUID);
                    SceneManager.GetSceneByPath(path);
                    EditorSceneManager.OpenScene(path, mode);
                }

                LevelHandler.SetCurrentLevelIndex(targetLevelNo);
            });
            _levelNoAsString = "";
        }

        #endregion

#endif
    }
}