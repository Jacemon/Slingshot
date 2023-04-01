using Lean.Localization;
using Tools;
using UnityEngine;

namespace Managers
{
    public class NotDestroyableManager : MonoBehaviour
    {
        public void SetLanguage(string languageName)
        {
            if (NotDestroyable.TryGetComponent("Localization", out LeanLocalization leanLocalization))
            {
                leanLocalization.CurrentLanguage = languageName;
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

        public void ShowRewardedAd()
        {
            if (NotDestroyable.TryGetComponent("Ad", out AdManager adManager))
            {
                adManager.ShowRewardedAd();
            }
        }

        public void LoadScene(string sceneName)
        {
            if (NotDestroyable.TryGetComponent("Transition", out SceneLoaderManager sceneLoaderManager))
            {
                sceneLoaderManager.LoadSceneWithTransition(sceneName);
            }
        }
            
        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}
