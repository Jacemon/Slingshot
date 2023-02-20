using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public List<GameObject> levels = new();
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
            prevButton.gameObject.SetActive(currentLevel != 0);
            nextButton.gameObject.SetActive(currentLevel != levels.Count - 1);
        }
        
        public void LoadLevel(int levelNumber)
        {
            if (levelNumber < 0 || levelNumber > levels.Count - 1)
            {
                Debug.Log("Edge");
                return;
            }
        
            Destroy(loadedLevel);
            loadedLevel = Instantiate(levels[levelNumber], startPosition, Quaternion.identity);
        
            currentLevel = levelNumber;
            
            CheckButtonsEnabled();
        
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
