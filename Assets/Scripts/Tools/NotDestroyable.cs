using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class NotDestroyable : MonoBehaviour
    {
        private static readonly Dictionary<string, GameObject> NotDestroyableGameObjects = new();

        private void Awake()
        {
            var key = gameObject.name;
            if (NotDestroyableGameObjects.ContainsKey(key))
            {
                Destroy(gameObject);
                return;
            }
            
            NotDestroyableGameObjects[key] = gameObject;
            DontDestroyOnLoad(NotDestroyableGameObjects[key]);
        }

        private void OnApplicationQuit()
        {
            NotDestroyableGameObjects.Remove(gameObject.name);
        }

        public static bool TryGetComponent<T>(string key, out T component)
        {
            if (TryGetGameObject(key, out var gameObject))
            {
                return gameObject.TryGetComponent(out component);
            }
            component = default;
            return false;
        }
        
        public static bool TryGetGameObject(string key, out GameObject gameObject)
        {
            return NotDestroyableGameObjects.TryGetValue(key, out gameObject);
        }
    }
}