// Pool.cs

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Root.Scripts.Pooling
{
    public class Pool
    {
        private static Transform _allPools;
        public static Transform AllPools => _allPools;
        public string Name;
        public int Size;
        public AssetReferenceGameObject AssetReference;
        private readonly Transform _parent;
        public GameObject Prefab;

        private readonly Queue<GameObject> _deactivatedObjects = new();
        private readonly List<GameObject> _activatedObjects = new();
        private readonly Dictionary<GameObject, List<Component>> _cachedComponents = new();


        public Pool()
        {
            if (_allPools == null) _allPools = new GameObject("All Pools").transform;
            _deactivatedObjects.Clear();
            _activatedObjects.Clear();
            _cachedComponents.Clear();

            _parent = new GameObject().transform;
            _parent.SetParent(_allPools);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private GameObject CreateObject()
        {
            var gameObject = Object.Instantiate(Prefab, _parent);
            var components = gameObject.GetComponents<Component>();
            _cachedComponents.Add(gameObject, new List<Component>(components));
            gameObject.SetActive(false);
            return gameObject;
        }

        public void WarmUp()
        {
            _parent.name = Name;
            for (var i = 0; i < Size; i++)
            {
                var obj = CreateObject();
                _deactivatedObjects.Enqueue(obj);
            }
        }

        public GameObject Get()
        {
            if (_deactivatedObjects.Count == 0)
            {
                var obj = CreateObject();
                _activatedObjects.Add(obj);
                return obj;
            }

            var gameObject = _deactivatedObjects.Dequeue();
            _activatedObjects.Add(gameObject);
            return gameObject;
        }

        public T Get<T>() where T : Component
        {
            var instance = Get();
            if (instance == null) return null;
            var components = _cachedComponents[instance];
            foreach (var component in components)
                if (component is T result)
                    return result;
            return null;
        }

        public void Return(GameObject gameObject)
        {
            if (!_activatedObjects.Contains(gameObject)) return;
            
            gameObject.transform.SetParent(_parent);
            gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            gameObject.SetActive(false);
            
            _activatedObjects.Remove(gameObject);
            _deactivatedObjects.Enqueue(gameObject);
        }
    }
}