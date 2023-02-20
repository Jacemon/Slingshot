using UnityEngine;

namespace Managers
{
    public class NotDestroyable : MonoBehaviour
    {
        private static GameObject _notDestroyable;
        
        public void Awake()
        {
            if (_notDestroyable != null)
            {
                Destroy(gameObject);
                return;
            }

            var notDestroyable = gameObject;
            
            _notDestroyable = notDestroyable;
            DontDestroyOnLoad(notDestroyable);
        }

        // public static GameObject notDestroyable;
        //
        // private static DontDestroyOnLoadManager _instance;
        //
        // private void Awake()
        // {
        //     if (_instance == null)
        //     {
        //         _instance = this;
        //         DontDestroyOnLoad(gameObject);
        //     }
        //     else
        //     {
        //         Destroy(gameObject);
        //     }
        // }
        //
        // public static DontDestroyOnLoadManager GetInstance()
        // {
        //     if (_instance == null)
        //     {
        //         GameObject singletonObject = Instantiate(notDestroyable);
        //         _instance = singletonObject.AddComponent<DontDestroyOnLoadManager>();
        //         DontDestroyOnLoad(singletonObject);
        //     }
        //
        //     return _instance;
        // }
    }
}