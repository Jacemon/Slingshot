using System.Linq;
using Entities.Levels;
using TMPro;
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
        [Space]
        public Vector2 startPosition = Vector2.zero;
        [Space] 
        public TextMeshProUGUI levelLabel;
        [Space]
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

            if (levels.ContainsKey(levelNumber))
            {
                Debug.Log($"Level {currentLevel} was loaded");
            }
            else
            {
                var closestKey = levels.Keys.Where(k => k <= currentLevel).Max();
                levels[currentLevel] = levels[closestKey];
                Debug.Log($"Level {currentLevel} was generated");
            }
            loadedLevel = Instantiate(levels[currentLevel], startPosition, Quaternion.identity);
            if (loadedLevel.TryGetComponent<GeneratedLevel>(out var level)) {
                level.levelNumber = currentLevel;
                level.Reload();
            }

            CheckButtonsEnabled();

            levelLabel.text = currentLevel.ToString();
            
            GlobalEventManager.OnLevelSwitched?.Invoke();
            Debug.Log($"Level has been switched. Current level: {currentLevel}");
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
