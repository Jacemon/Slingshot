using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public void Awake()
        {
            GlobalEventManager.OnLoad?.Invoke();
        }

        private void OnDestroy()
        {
            GlobalEventManager.OnSave?.Invoke();
            PlayerPrefs.Save();
        }
    }
}