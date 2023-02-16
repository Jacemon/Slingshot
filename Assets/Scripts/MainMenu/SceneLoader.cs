using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class SceneLoader : MonoBehaviour
    {
        public void QuitApplication()
        {
            Application.Quit();
        }
        
        public void LoadLevelScene()
        {
            SceneManager.LoadScene("Levels");
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
