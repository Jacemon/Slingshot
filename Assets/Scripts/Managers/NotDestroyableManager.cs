using Lean.Localization;
using Tools;
using UnityEngine;

namespace Managers
{
    public class NotDestroyableManager : MonoBehaviour
    {
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
                audioManager.effectsVolumeSwitch.Value = isOn;
            }
        }
        
        public void ToggleMusic(bool isOn)
        {
            if (NotDestroyable.TryGetComponent("Audio", out AudioManager audioManager))
            {
                audioManager.musicVolumeSwitch.Value = isOn;
            }
        }
        
        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}
