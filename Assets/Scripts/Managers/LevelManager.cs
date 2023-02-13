using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> levels = new();
    public int currentLevel;

    public Vector2 startPosition = Vector2.zero;
    
    [SerializeField]
    private GameObject loadedLevel;

    public void Awake()
    {
        LoadLevel(0);
    }

    public void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber < 0 || levelNumber > levels.Count - 1)
        {
            Debug.Log("Edge");
            return;
        }
        
        Destroy(loadedLevel);
        loadedLevel = Instantiate(levels[levelNumber], startPosition, Quaternion.identity);
        
        currentLevel = levelNumber;
        
        Debug.Log($"Current level: {currentLevel}");
    }
    
    public void NextLevel()
    {
        LoadLevel(currentLevel + 1);
    }

    public void PreviousLevel()
    {
        LoadLevel(currentLevel - 1);
    }
}
