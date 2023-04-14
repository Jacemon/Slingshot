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
        [SerializeField, ReadOnlyInspector]
        private Level loadedLevel;

        private void Awake()
        {
            CheckGUI();
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
            LoadLevel(maxAvailableLevel.Value);
            Debug.Log($"MaxAvailableLevel: -> {maxAvailableLevel.Value}");
        }
        
        private void CheckGUI()
        {
            levelLabel.text = currentLevel.Value.ToString();

            prevButton.gameObject.SetActive(currentLevel.Value != levels.Keys.Min());
            nextButton.gameObject.SetActive(currentLevel.Value != levels.Keys.Max() 
                                            && currentLevel.Value != maxAvailableLevel.Value);
            buyButton.gameObject.SetActive(currentLevel.Value != levels.Keys.Max() 
                                           && currentLevel.Value == maxAvailableLevel.Value);
        }

        public void LoadLevel(int levelNumber)
        {
            Debug.Log($"Start loading level {levelNumber}...");
            
            if (levelNumber < levels.Keys.Min() || levelNumber > levels.Keys.Max())
            {
                Debug.Log("Edge");
                return;
            }

            if (levelNumber > maxAvailableLevel.Value)
            {
                currentLevel.Value = maxAvailableLevel.Value;
                Debug.Log("Not available level");
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
                Destroy(loadedLevel.gameObject);
            }
            loadedLevel = Instantiate(levels[levelNumber]);

            GlobalEventManager.OnLevelLoaded?.Invoke(loadedLevel);
            
            Debug.Log($"End loading level {currentLevel.Value}...");
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
