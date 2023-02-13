using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLevelScene()
    {
        SceneManager.LoadScene("Levels");
    }
}
