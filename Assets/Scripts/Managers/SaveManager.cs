using System;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private void Awake()
        {
            GlobalEventManager.OnLoad?.Invoke();
        }

        private void OnDestroy()
        {
            GlobalEventManager.OnSave?.Invoke();
            PlayerPrefs.Save();
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