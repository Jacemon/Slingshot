using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoader : MonoBehaviour
{
    public List<GameObject> levels = new();
    public int currentLevel;

    public Vector2 startPosition = Vector2.zero;
    public float levelOffsetX = 5.04f;
    public float levelSwapSpeed = 4.0f;
    
    // todo пока что не линкедлист
    [SerializeField]
    private List<GameObject> loadedLevels = new();

    [SerializeField]
    private Vector2 viewPoint;
    [FormerlySerializedAs("loadedLevelsMaxCount")] [FormerlySerializedAs("loadedLevelMaxCount")] [SerializeField]
    private int maxLevelsToLoad = 3;

    public void Awake()
    {
        LoadLevel(0);
    }

    public void Update()
    {
        // var position = loadedLevels[0].transform.position;
        // Vector3 offset = position - Vector3.MoveTowards(
        //     position,
        //     viewPoint + new Vector2(levelOffsetX, 0),
        //     Time.deltaTime * levelSwapSpeed
        // );
        // foreach (GameObject level in loadedLevels)
        // {
        //     level.transform.position += offset;
        // }
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
        
        loadedLevels.ForEach(Destroy);
        loadedLevels.Clear();
        
        int leftHalf = maxLevelsToLoad / 2;
        int rightHalf = maxLevelsToLoad - leftHalf;
        int leftBorder = levelNumber - leftHalf < 0 ? 0 : levelNumber - leftHalf;
        int rightBorder = levelNumber + rightHalf - 1 > levels.Count ? levels.Count : levelNumber + rightHalf;

        for (int i = leftBorder; i <= rightBorder; i++)
        {
            loadedLevels.Add(Instantiate(levels[i], new Vector2((i - levelNumber) * levelOffsetX, 0),
                Quaternion.identity));
        }

        currentLevel = levelNumber;
        
        Debug.Log($"Current level: {currentLevel}");
    }
    
    public void NextLevel()
    {
        LoadLevel(currentLevel + 1);
        // if (currentLevel < levels.Count)
        // {
        //     currentLevel++;
        //     if (loadedLevels.Count == loadedLevelsMaxCount)
        //     {
        //         Destroy(loadedLevels[0]);
        //     }
        //     loadedLevels.RemoveAt(0);
        //     loadedLevels.Add(Instantiate(levels[currentLevel + 1]));
        //     viewPoint.x += levelOffsetX;
        //     
        //     Debug.Log($"Current level: {currentLevel}");
        // }
        // else
        // {
        //     Debug.Log("Right edge");
        // }
    }

    public void PreviousLevel()
    {
        LoadLevel(currentLevel - 1);
        // if (currentLevel > 0)
        // {
        //     currentLevel--;
        //     if (loadedLevels.Count == maxLevelsToLoad)
        //     {
        //         Destroy(loadedLevels[^1]);
        //     }
        //     loadedLevels.RemoveAt(loadedLevels.Count - 1);
        //     loadedLevels.Insert(0, Instantiate(levels[currentLevel - 1]));
        //     viewPoint.x -= levelOffsetX;
        //     
        //     Debug.Log($"Current level: {currentLevel}");
        // }
        // else
        // {
        //     Debug.Log("Left edge");
        // }
    }
}
