using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneLoaderManager : MonoBehaviour
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
            SceneManager.LoadScene("MainMenu");
        }
        
        public void ReloadActiveScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
