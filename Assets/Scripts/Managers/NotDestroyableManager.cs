using Lean.Localization;
using Tools;
using Tools.Interfaces;
using UnityEngine;

namespace Managers
{
    public class NotDestroyableManager : MonoBehaviour, ISavable
    {
        public void Awake()
        {
            GlobalEventManager.UnityEvents.OnSave.AddListener(SaveData);
            GlobalEventManager.UnityEvents.OnLoad.AddListener(LoadData);
        }

        public void SetLanguage(string languageName)
        {
            if (NotDestroyable.TryGetComponent("LeanLocalization", out LeanLocalization leanLocalization))
            {
                leanLocalization.CurrentLanguage = languageName;
            }
        }

        public void LoadScene(string sceneName)
        {
            if (NotDestroyable.TryGetComponent("Transition", out SceneLoaderManager sceneLoaderManager))
            {
                sceneLoaderManager.LoadSceneWithTransition(sceneName);
            }
        }

        public void ToggleEffects(bool isOn)
        {
            if (NotDestroyable.TryGetComponent("Audio", out AudioManager audioManager))
            {
                audioManager.effectsVolumeSwitch = isOn;
            }
        }
        
        public void ToggleMusic(bool isOn)
        {
            if (NotDestroyable.TryGetComponent("Audio", out AudioManager audioManager))
            {
                audioManager.musicVolumeSwitch = isOn;
            }
        }
        
        public void QuitApplication()
        {
            Application.Quit();
        }
        
        // Saving
        public void SaveData()
        {
            if (NotDestroyable.TryGetComponent("LeanLocalization", out LeanLocalization leanLocalization))
            {
                PlayerPrefs.SetString("language", leanLocalization.CurrentLanguage);
            }
            if (NotDestroyable.TryGetComponent("Audio", out AudioManager audioManager))
            {
                PlayerPrefs.SetInt("music", audioManager.musicVolumeSwitch ? 1 : 0);
                PlayerPrefs.SetInt("effects", audioManager.effectsVolumeSwitch ? 1 : 0);
            }
        }

        public void LoadData()
        {
            if (NotDestroyable.TryGetComponent("LeanLocalization", out LeanLocalization leanLocalization))
            {
                leanLocalization.CurrentLanguage = PlayerPrefs.GetString("language");
            }
            if (NotDestroyable.TryGetComponent("Audio", out AudioManager audioManager))
            {
                audioManager.musicVolumeSwitch = PlayerPrefs.GetInt("music") != 0;
                audioManager.effectsVolumeSwitch = PlayerPrefs.GetInt("effects") != 0;
            }
        }
        
        public void ReloadData() { }
    }
}
