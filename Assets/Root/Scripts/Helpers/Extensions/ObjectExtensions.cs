// ObjectExtensions.cs

using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace Root.Scripts.Helpers.Extensions
{
    public static partial class Extensions
    {
        public static bool ValidateInterface<T1, T2>(this T1 me, out T2 result)
        {
            result = default;

            if (!((IList)me.GetType().GetInterfaces()).Contains(typeof(T2))) return false;

            result = (T2)(object)me;
            return true;
        }

#if UNITY_EDITOR

        /// <summary>
        ///     Updates given serialized objects serialized lists
        /// </summary>
        /// <param name="me">caller So</param>
        /// <typeparam name="T">Type of So</typeparam>
        public static void UpdateValues<T>(this T me) where T : ScriptableObject
        {
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(me);
            AssetDatabase.SaveAssets();
        }
        
        /// <summary>
        ///     Saves ScriptableObject to Resources folder
        /// </summary>
        /// <typeparam name="T">type of ScriptableObject</typeparam>
        /// <param name="me">ScriptableObject</param>
        /// <param name="subFolder">path to Resources folder</param>
        /// <param name="assetName">name for asset to save</param>
        /// <returns>saved ScriptableObject and count of saved ScriptableObjects</returns>
        /// <example>newLevel.SaveAsset("Resources/Levels");</example>
        public static (T, int) SaveAsset<T>(this T me, string subFolder = "Assets/Resources/",
            string assetName = default)
            where T : ScriptableObject
        {
            var path = AssetDatabase.GetAssetPath(me);
            var count = 1;
            if (string.IsNullOrEmpty(path))
            {
                path = subFolder;
                if (assetName == default)
                {
                    var name = me.GetType().Name.Split('_').Last();
                    while (AssetDatabase.LoadAssetAtPath<T>($"{path}/{name}_{count:00}.asset") != null)
                        count++;
                    path = $"{path}/{name}_{count:00}.asset";
                }
                else
                {
                    path = $"{path}/{assetName}.asset";
                }
            }

            AssetDatabase.CreateAsset(me, path);
            AssetDatabase.SaveAssets();
            return (me, count);
        }
        
        
        /// <summary>
        ///     Opens given folder path in project window, only usable in editor mode
        /// </summary>
        /// <param name="folderPath">given folder path</param>
        /// <returns>returns true if folder path is valid</returns>
        public static void OpenFolder(this string folderPath)
        {
            if (!AssetDatabase.IsValidFolder(folderPath)) return;
            // open folder and show its content in project window
            var firstAssetInFolder = AssetDatabase.FindAssets("", new[] { folderPath }).First();
            if (firstAssetInFolder == null) return;
            var assetPath = AssetDatabase.GUIDToAssetPath(firstAssetInFolder);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        }

        public static AddressableAssetEntry SetAsAddressable(this string path)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var group = settings.DefaultGroup;
            var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group);
            entry.address = path;
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, group, true);
            return entry;
        }
        
#endif
    }
}