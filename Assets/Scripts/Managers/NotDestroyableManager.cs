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
                leanLocalization.SetCurrentLanguage(languageName);
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
