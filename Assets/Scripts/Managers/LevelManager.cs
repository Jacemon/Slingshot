using System;
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

        private void Awake()
        {
            GlobalEventManager.UnityEvents.OnLoad.AddListener(LoadData);
            GlobalEventManager.UnityEvents.OnSave.AddListener(SaveData);
        }

        private void OnEnable()
        {
            GlobalEventManager.onLevelUp += LevelUp;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.onLevelUp -= LevelUp;
        }
        
        private void CheckGUI()
        {
            levelLabel.text = currentLevel.ToString();
    
            prevButton.gameObject.SetActive(currentLevel != levels.Keys.Min());
            nextButton.gameObject.SetActive(currentLevel != levels.Keys.Max() && currentLevel != maxAvailableLevel);
            buyButton.gameObject.SetActive(currentLevel != levels.Keys.Max() && currentLevel == maxAvailableLevel);
        }

        private int LevelUp(int levelCount)
        {
            maxAvailableLevel += levelCount;
            NextLevel();
            
            Debug.Log($"Level: {maxAvailableLevel - levelCount} -> {maxAvailableLevel}");
            
            return maxAvailableLevel;
        }
        
        public void LoadLevel(int levelNumber)
        {
            Debug.Log($"Start loading level {levelNumber}...");
            
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
            CheckGUI();

            // Find the left level closest to the current one
            if (!levels.ContainsKey(levelNumber))
            {
                var closestKey = levels.Keys.Where(k => k < currentLevel).Max();
                levels[currentLevel] = levels[closestKey];
            }

            // Find all the Level components and configure them
            var levelScripts = levels[currentLevel].GetComponents<Level>();
            if (levelScripts.Length == 0)
            {
                Debug.Log($"Level {currentLevel} has not Level scripts...");
                return;
            }
            foreach (var level in levelScripts)
            {
                level.levelNumber = currentLevel;
            }

            // Destroy old and create new level
            if (loadedLevel != null)
            {
                Destroy(loadedLevel.gameObject);
            }
            loadedLevel = Instantiate(levelScripts[0], startPosition, Quaternion.identity);

            GlobalEventManager.onLevelLoad?.Invoke();
            
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
            
            Debug.Log($"LevelManager was loaded: {currentLevel}/{maxAvailableLevel}");
        }

        public void ReloadData()
        {
            LoadLevel(currentLevel);
        }
    }
}
