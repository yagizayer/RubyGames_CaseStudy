using Root.Scripts.EventHandling.BasicPassableData;
using Root.Scripts.Helpers.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Root.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Level_", menuName = "DataHolders/LevelData")]
    public class LevelData : ScriptableObject, IPassableData
    {
        [SerializeField]
        public AssetReference levelReference;

        [ReadOnly]
        [SerializeField]
        public int levelNumber;

        [field: HideInInspector]
        [field: SerializeField]
        public Scene MyScene { get; private set; }


#if UNITY_EDITOR

        // Handle saving Here
        private void HandleSave(Scene scene, string path)
        {
        }

        private void OnEnable() => EditorSceneManager.sceneSaved += OnSceneSaved;

        private void OnDisable() => EditorSceneManager.sceneSaved -= OnSceneSaved;

        // calls after scene saved
        private void OnSceneSaved(Scene scene)
        {
            if (!ValidateScene(scene, out var path)) return;

            HandleSave(scene, path);

            this.UpdateValues();
        }

        private bool ValidateScene(Scene scene, out string path)
        {
            path = AssetDatabase.GUIDToAssetPath(levelReference.AssetGUID);
            MyScene = SceneManager.GetSceneByPath(path);
            return scene == MyScene;
        }
#endif
    }
}