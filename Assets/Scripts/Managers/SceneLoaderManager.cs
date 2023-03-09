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
        
        public void LoadLevelsScene()
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
        
        // TODO: temp
        public void DeleteAllProgress()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
