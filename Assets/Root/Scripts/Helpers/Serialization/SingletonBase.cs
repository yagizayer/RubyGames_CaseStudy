// SingletonBase.cs

using UnityEngine;

namespace Root.Scripts.Helpers.Serialization
{
    public abstract class SingletonBase<TManager> : MonoBehaviour where TManager : MonoBehaviour
    {
        private static TManager _instance;

        public static TManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<TManager>();
                if (_instance != null) return _instance;
                var singleton = new GameObject($"({typeof(TManager).Name})");
                _instance = singleton.AddComponent<TManager>();
                DontDestroyOnLoad(singleton);
                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as TManager;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }
}