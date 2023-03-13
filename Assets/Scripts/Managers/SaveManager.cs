using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("Start loading...");
            GlobalEventManager.onLoad?.Invoke();
            Debug.Log("End loading...");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                return;
            }
            OnDisable();
        }

        private void OnDisable()
        {
            Debug.Log("Start saving...");
            GlobalEventManager.onSave?.Invoke();
            PlayerPrefs.Save();
            Debug.Log("End saving...");
        }
    }
}