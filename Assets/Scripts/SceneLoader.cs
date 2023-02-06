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
    
    private readonly LinkedList<GameObject> _loadedLevels = new();

    [SerializeField]
    private Vector2 _viewPoint;
    [FormerlySerializedAs("_loadedLevelNumber")] [SerializeField]
    private int _loadedLevelMaxCount = 3;
    
    // todo сделать чтобы загружалось определённное количество уровней, а не только 3
    public void Awake()
    {
        Debug.Log($"Current level: {currentLevel}");
        if (currentLevel > 0)
        {
            _loadedLevels.AddLast(Instantiate(levels[currentLevel - 1], startPosition + new Vector2(-levelOffsetX, 0),
                Quaternion.identity));
            Debug.Log("0");
        }
        _loadedLevels.AddLast(Instantiate(levels[currentLevel], startPosition, Quaternion.identity));
            Debug.Log("1");
        if (currentLevel < levels.Count)
        {
            _loadedLevels.AddLast(Instantiate(levels[currentLevel + 1], startPosition + new Vector2(levelOffsetX, 0),
                Quaternion.identity));
            Debug.Log("2");
        }
    }

    // todo тоже самое что сверху
    public void Update()
    {
        var position = _loadedLevels.First.Value.transform.position;
        Vector3 offset = position - Vector3.MoveTowards(
            position,
            _viewPoint + new Vector2(levelOffsetX, 0),
            Time.deltaTime * levelSwapSpeed
        );
        foreach (GameObject level in _loadedLevels)
        {
            level.transform.position += offset;
        }
    }

    public void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        if (currentLevel < levels.Count)
        {
            currentLevel++;
            if (_loadedLevels.Count == _loadedLevelMaxCount)
            {
                Destroy(_loadedLevels.First.Value);
            }
            _loadedLevels.RemoveFirst();
            _loadedLevels.AddLast(Instantiate(levels[currentLevel + 1]));
            _viewPoint.x += levelOffsetX;
        }
        else
        {
            Debug.Log("Right edge");
        }
    }

    public void PreviousLevel()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            if (_loadedLevels.Count == _loadedLevelMaxCount)
            {
                Destroy(_loadedLevels.Last.Value);
            }
            _loadedLevels.RemoveLast();
            _loadedLevels.AddFirst(Instantiate(levels[currentLevel - 1]));
            _viewPoint.x -= levelOffsetX;
        }
        else
        {
            Debug.Log("Left edge");
        }
    }
}
