using System.Linq;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public IntGameObjectDictionary levels = new();
        public int currentLevel;
        [Space] 
        public Button nextButton;
        public Button prevButton;

        public Vector2 startPosition = Vector2.zero;
    
        [SerializeField]
        private GameObject loadedLevel;

        public void Awake()
        {
            LoadLevel(0);

            CheckButtonsEnabled();
        }

        private void CheckButtonsEnabled()
        {
            prevButton.gameObject.SetActive(currentLevel != levels.Keys.Min());
            nextButton.gameObject.SetActive(currentLevel != levels.Keys.Max());
        }
        
        public void LoadLevel(int levelNumber)
        {
            if (levelNumber < levels.Keys.Min() || levelNumber > levels.Keys.Max())
            {
                Debug.Log("Edge");
                return;
            }
        
            currentLevel = levelNumber;
            
            Destroy(loadedLevel);
            if (levels.TryGetValue(levelNumber, out var levelToLoad))
            {
                Debug.Log($"Level {currentLevel} was loaded");
                loadedLevel = Instantiate(levelToLoad, startPosition, Quaternion.identity);
            }
            else
            {
                Debug.Log($"Level {currentLevel} was generated");
                GenerateLevel();
            }

            CheckButtonsEnabled();
        
            GlobalEventManager.OnLevelSwitched?.Invoke();
            Debug.Log($"Level has been switched. Current level: {currentLevel}");
        }

        private void GenerateLevel()
        {
            
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
}
