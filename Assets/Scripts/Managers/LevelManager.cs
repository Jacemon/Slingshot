using System.Linq;
using Entities.Levels;
using TMPro;
using Tools;
using Tools.Dictionaries;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class LevelManager : MonoBehaviour, ISavable
    {
        public IntGameObjectDictionary levels = new();
        public int currentLevel;
        public int maxAvailableLevel;
        [Space] 
        public Button nextButton;
        public Button prevButton;
        public Button buyButton;
        [Space]
        public Vector2 startPosition = Vector2.zero;
        [Space] 
        public TextMeshProUGUI levelLabel;
        [Space]
        [SerializeField]
        private GameObject loadedLevel;

        public void Awake()
        {
            GlobalEventManager.OnLevelUpped.AddListener(Reload);
            
            GlobalEventManager.OnLoad.AddListener(Load);
            GlobalEventManager.OnSave.AddListener(Save);
            
            LoadLevel(0);

            CheckButtonsEnabled();
        }

        private void CheckButtonsEnabled()
        {
            prevButton.gameObject.SetActive(currentLevel != levels.Keys.Min());
            nextButton.gameObject.SetActive(currentLevel != levels.Keys.Max() && currentLevel != maxAvailableLevel);
            buyButton.gameObject.SetActive(currentLevel != levels.Keys.Max() && currentLevel == maxAvailableLevel);
        }
        
        public void LoadLevel(int levelNumber)
        {
            if (levelNumber < levels.Keys.Min() || levelNumber > levels.Keys.Max())
            {
                Debug.Log("Edge");
                return;
            }

            if (levelNumber > maxAvailableLevel)
            {
                currentLevel = maxAvailableLevel;
                Debug.Log("Not available level");
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

        public void Save()
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            PlayerPrefs.SetInt("maxAvailableLevel", maxAvailableLevel);
        }

        public void Load()
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
            maxAvailableLevel = PlayerPrefs.GetInt("maxAvailableLevel");
            Reload();
        }

        public void Reload()
        {
            LoadLevel(currentLevel);
        }
    }
}
