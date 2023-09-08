// PoolManager.cs

using System;
using System.Collections.Generic;
using System.Reflection;
using Root.Scripts.Helpers.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Root.Scripts.Pooling
{
    public static class PoolManager
    {
        private static readonly Dictionary<string, Pool> Pools = new();

        #region Initialization

        public static void Initialize()
        {
            Pools.Clear();
            var pools = GetPoolReferences();
            foreach (var pool in pools)
            {
                if (Pools.ContainsKey(pool.Name)) return;
                Pools.Add(pool.Name, pool);

                Addressables.LoadAssetAsync<GameObject>(pool.AssetReference).Completed += handle =>
                {
                    if (handle.Status != AsyncOperationStatus.Succeeded) return;
                    pool.Prefab = handle.Result;
                    pool.WarmUp();
                };
            }
        }

        private static IEnumerable<Pool> GetPoolReferences()
        {
            var result = new List<Pool>();

            // in all assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                // get all types
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    // get all fields
                    var fields = type.GetFields();
                    foreach (var field in fields)
                    {
                        // get all fields with PoolAttribute
                        var poolAttribute = field.GetCustomAttribute<PoolAttribute>();
                        if (poolAttribute == null) continue;

                        // get all occurrences of this type
                        var allOccurrences = Resources.FindObjectsOfTypeAll(type);
                        foreach (var occurrence in allOccurrences)
                        {
                            // if field is not AssetReferenceGameObject, skip
                            if (field.GetValue(occurrence) is not AssetReferenceGameObject assetReference) continue;

                            // add to result
                            result.Add(new Pool
                            {
                                AssetReference = assetReference,
                                Name = poolAttribute.Pool.ToPoolName(),
                                Size = poolAttribute.PoolSize
                            });
                        }
                    }
                }
            }

            return result.ToArray();
        }

        #endregion

        public static GameObject Get(string poolName) =>
            Pools.ContainsKey(poolName) ? Pools[poolName].Get() : null;

        public static T Get<T>(string poolName) where T : Component =>
            Pools.ContainsKey(poolName) ? Pools[poolName].Get<T>() : null;

        public static void Return(string poolName, GameObject instance) =>
            Pools[poolName].Return(instance);
    }
}