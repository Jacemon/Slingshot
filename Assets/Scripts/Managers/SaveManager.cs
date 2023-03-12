using System;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("Start loading...");
            GlobalEventManager.UnityEvents.OnLoad?.Invoke();
            Debug.Log("End loading...");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                return;
            }
            OnApplicationQuit();
        }

        private void OnDisable()
        {
            OnApplicationQuit();
        }

        private void OnApplicationQuit()
        {
            Debug.Log("Start saving...");
            GlobalEventManager.UnityEvents.OnSave?.Invoke();
            PlayerPrefs.Save();
            Debug.Log("End saving...");
        }
    }
}