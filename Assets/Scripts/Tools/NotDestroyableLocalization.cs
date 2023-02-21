using Lean.Localization;
using UnityEngine;

namespace Tools
{
    public class NotDestroyableLocalization : MonoBehaviour
    {
        private static GameObject _notDestroyable;

        private void Awake()
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

        public static void SetLanguage(string languageName)
        {
            _notDestroyable.GetComponent<LeanLocalization>().SetCurrentLanguage(languageName);
        }
    }
}