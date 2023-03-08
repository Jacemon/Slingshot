using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("Start loading...");
            GlobalEventManager.UnityEvents.OnLoad?.Invoke();
            Debug.Log("End loading...");
        }

        private void OnDestroy()
        {
            Debug.Log("Start saving...");
            GlobalEventManager.UnityEvents.OnSave?.Invoke();
            PlayerPrefs.Save();
            Debug.Log("End saving...");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                return;
            }
            OnDestroy();
        }
        
        private void OnApplicationQuit()
        {
            OnDestroy();
        }
    }
}