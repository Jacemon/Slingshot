using System.Linq;
using Entities.Levels;
using TMPro;
using Tools.Dictionaries;
using Tools.ScriptableObjects;
using Tools.ScriptableObjects.Reference;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public IntGameObjectDictionary levels = new();
        public IntReference currentLevel;
        public IntReference maxAvailableLevel;
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
            LoadLevel(currentLevel.Value);
        }

        private void OnEnable()
        {
            maxAvailableLevel.onValueChanged += OnMaxAvailableLevelChanged;
        }
        
        private void OnDisable()
        {
            maxAvailableLevel.onValueChanged -= OnMaxAvailableLevelChanged;
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
            nextButton.gameObject.SetActive(currentLevel.Value != levels.Keys.Max() && currentLevel.Value != maxAvailableLevel.Value);
            buyButton.gameObject.SetActive(currentLevel.Value != levels.Keys.Max() && currentLevel.Value == maxAvailableLevel.Value);
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
            
            currentLevel.Value = levelNumber;
            CheckGUI();

            // Find the left level closest to the current one
            if (!levels.ContainsKey(levelNumber))
            {
                var closestKey = levels.Keys.Where(k => k < currentLevel.Value).Max();
                levels[currentLevel.Value] = levels[closestKey];
            }

            // Find all the Level components and configure them
            var levelScripts = levels[currentLevel.Value].GetComponents<Level>();
            if (levelScripts.Length == 0)
            {
                Debug.Log($"Level {currentLevel.Value} has not Level scripts...");
                return;
            }
            foreach (var level in levelScripts)
            {
                level.levelNumber = currentLevel.Value;
            }

            // Destroy old and create new level
            if (loadedLevel != null)
            {
                Destroy(loadedLevel.gameObject);
            }
            loadedLevel = Instantiate(levelScripts[0], startPosition, Quaternion.identity);

            GlobalEventManager.onLevelLoad?.Invoke();
            
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
