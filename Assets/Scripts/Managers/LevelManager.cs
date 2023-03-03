using System.Linq;
using Entities.Levels;
using TMPro;
using Tools.Dictionaries;
using Tools.Interfaces;
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
        private Level loadedLevel;

        public void Awake()
        {
            GlobalEventManager.OnLevelUp.AddListener(LevelUp);
            
            GlobalEventManager.OnLoad.AddListener(LoadData);
            GlobalEventManager.OnSave.AddListener(SaveData);
            
            ReloadData();
        }

        private void CheckButtonsEnabled()
        {
            prevButton.gameObject.SetActive(currentLevel != levels.Keys.Min());
            nextButton.gameObject.SetActive(currentLevel != levels.Keys.Max() && currentLevel != maxAvailableLevel);
            buyButton.gameObject.SetActive(currentLevel != levels.Keys.Max() && currentLevel == maxAvailableLevel);
        }

        private void LevelUp()
        {
            maxAvailableLevel++;
            ReloadData();
        }
        
        public void LoadLevel(int levelNumber)
        {
            Debug.Log($"Start loading level {currentLevel}...");
            
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

            if (loadedLevel != null)
            {
                Destroy(loadedLevel.gameObject);
            }

            if (!levels.ContainsKey(levelNumber))
            {
                var closestKey = levels.Keys.Where(k => k < currentLevel).Max();
                levels[currentLevel] = levels[closestKey];
            }
            
            if (levels[currentLevel].TryGetComponent(out Level level)) {
                level.levelNumber = currentLevel;
            }
            loadedLevel = Instantiate(level, startPosition, Quaternion.identity);

            CheckButtonsEnabled();

            levelLabel.text = currentLevel.ToString();
            
            GlobalEventManager.OnLevelLoad?.Invoke();
            
            Debug.Log($"End loading level {currentLevel}...");
        }

        public void NextLevel()
        {
            LoadLevel(currentLevel + 1);
        }

        public void PreviousLevel()
        {
            LoadLevel(currentLevel - 1);
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            PlayerPrefs.SetInt("maxAvailableLevel", maxAvailableLevel);
        }

        public void LoadData()
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
            maxAvailableLevel = PlayerPrefs.GetInt("maxAvailableLevel");
            ReloadData();
        }

        public void ReloadData()
        {
            LoadLevel(currentLevel);
        }
    }
}
