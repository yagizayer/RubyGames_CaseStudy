using Root.Scripts.Helpers.Extensions;
using Root.Scripts.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneTemplate;
#endif

namespace Root.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelCreator", menuName = "Handlers/LevelCreationHandler")]
    public class LevelCreationHandler : ScriptableObject
    {
#if UNITY_EDITOR

        [Title("Level Creator")]
        public SceneTemplateAsset levelBaseScene;

        [ShowIf("@levelHandler == null")]
        public LevelHandler levelHandler;

        [FolderPath]
        public string levelDatasPath = "Assets/Resources/Levels";

        [FolderPath]
        public string levelScenesPath = "Assets/ProjectRootFolder/Scenes/AllLevels";

        [Button]
        public void CreateLevel()
        {
            GameManager.UnloadLevels(onUnloadComplete: () =>
            {
                var levelName = $"Level_{levelHandler.allLevels.Count + 1:00}.unity";
                var levelPath = $"{levelScenesPath}/{levelName}";

                foreach (var dependency in levelBaseScene.dependencies)
                    dependency.instantiationMode = TemplateInstantiationMode.Reference;

                var newLevelScene = SceneTemplateService.Instantiate(levelBaseScene, true, levelPath);

                var newLevelData = CreateInstance<LevelData>();
                newLevelData.levelReference = new AssetReference(AssetDatabase.AssetPathToGUID(levelPath));
                newLevelData.levelNumber = levelHandler.allLevels.Count + 1;
                newLevelData.SaveAsset(levelDatasPath, $"Level_{newLevelData.levelNumber:00}");

                levelHandler.allLevels.Add(newLevelData);
                levelHandler.OnValidate();
                levelHandler.UpdateValues();

                var addressableAssetEntry = levelPath.SetAsAddressable();
                addressableAssetEntry.address = levelName;
                addressableAssetEntry.SetLabel("Level", true, true);
            });
        }

#endif
    }
}