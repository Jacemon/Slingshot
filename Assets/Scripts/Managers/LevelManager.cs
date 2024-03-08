using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Entities.Levels;
using TMPro;
using Tools;
using Tools.ScriptableObjects.References;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializedDictionary("Level number", "Level prefab")]
        public SerializedDictionary<int, Level> levels;
        [Space]
        public IntReference currentLevel;
        public IntReference maxAvailableLevel;
        [Space]
        public Button nextButton;
        public Button prevButton;
        public Button buyButton;
        [Space]
        public TextMeshProUGUI levelLabel;
        [Space]
        [SerializeField]
        [ReadOnlyInspector]
        private Level loadedLevel;

        public Action<Level, bool> LevelComplete;
        
        private void Awake()
        {
            CheckGUI();
            OnMaxAvailableLevelChanged();
            LoadLevel(currentLevel.Value);
        }

        private void OnEnable()
        {
            maxAvailableLevel.OnValueChanged += OnMaxAvailableLevelChanged;
            currentLevel.OnValueChanged += CheckGUI;
        }

        private void OnDisable()
        {
            maxAvailableLevel.OnValueChanged -= OnMaxAvailableLevelChanged;
            currentLevel.OnValueChanged -= CheckGUI;
        }

        private void OnMaxAvailableLevelChanged()
        {
            if (maxAvailableLevel.Value > levels.Keys.Max())
            {
                maxAvailableLevel.Value = levels.Keys.Max();
            } else if (maxAvailableLevel.Value < levels.Keys.Min())
            {
                maxAvailableLevel.Value = levels.Keys.Min();
            }
            // LoadLevel(maxAvailableLevel.Value);
            Debug.Log($"MaxAvailableLevel: -> {maxAvailableLevel.Value}");
        }

        private void CheckGUI() // TODO move to LevelUIController
        {
            levelLabel.text = currentLevel.Value.ToString();

            prevButton.gameObject.SetActive(currentLevel.Value != levels.Keys.Min());
            nextButton.gameObject.SetActive(currentLevel.Value != levels.Keys.Max()
                                            && currentLevel.Value != maxAvailableLevel.Value);
            // buyButton.gameObject.SetActive(currentLevel.Value != levels.Keys.Max()
            //                                && currentLevel.Value == maxAvailableLevel.Value);
        }
        
        public void LoadLevel(int levelNumber)
        {
            Debug.Log($"Start loading level {levelNumber}...");

            if (levelNumber < levels.Keys.Min())
            {
                Debug.Log("Min edge");
                LoadLevel(levels.Keys.Min());
                return;
            } 
            if (levelNumber > levels.Keys.Max())
            {
                Debug.Log("Max edge");
                LoadLevel(levels.Keys.Max());
                return;
            }
            if (levelNumber > maxAvailableLevel.Value)
            {
                Debug.Log("Not available level");
                LoadLevel(maxAvailableLevel.Value);
                return;
            }

            // Find the left level closest to the current one
            if (!levels.ContainsKey(levelNumber))
            {
                var closestKey = levels.Keys.Where(k => k < levelNumber).Max();
                levels[levelNumber] = levels[closestKey];
            }

            // Configure Level component if exists
            levels[levelNumber].levelNumber = levelNumber;

            currentLevel.Value = levelNumber;

            // Destroy old and create new level
            if (loadedLevel != null)
            {
                loadedLevel.LevelComplete -= CheckLevelComplete;
            }
            if (loadedLevel != null) Destroy(loadedLevel.gameObject);
            loadedLevel = Instantiate(levels[levelNumber]);
            loadedLevel.LevelComplete += CheckLevelComplete;
            
            GlobalEventManager.OnLevelLoaded?.Invoke(loadedLevel);

            Debug.Log($"End loading level {currentLevel.Value}...");
        }

        private void CheckLevelComplete(bool complete)
        {
            LevelComplete?.Invoke(loadedLevel, complete);
            
            if (complete && maxAvailableLevel.Value == currentLevel.Value)
            {
                maxAvailableLevel.Value++;
            }
        }

        public void StartLevel()
        {
            loadedLevel.StartLevel();
        }
        
        public void ReloadLevel()
        {
            LoadLevel(currentLevel.Value);
        }
        
        public void NextLevel()
        {
            LoadLevel(currentLevel.Value + 1);
        }

        public void PreviousLevel()
        {
            LoadLevel(currentLevel.Value - 1);
        }
    }
}