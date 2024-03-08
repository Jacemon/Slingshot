using Managers.AnimatedUI;
using Managers.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.UIControllers
{
    public class LevelUIController : MonoBehaviour
    {
        public LevelManager levelManager;
        [Space]
        public AnimatedUIToggle winPopUp;
        public AnimatedUIToggle losePopUp;
        [Space]
        public Button startLevelButton;
        public Button restartLevelButton;
        public Button nextLevelButton;
        public Button exitLevelButton;
        [Space]
        public Image curtain;
        [Space]
        public InteractiveController interactiveController;
        
        private void Start()
        {
            LoadLevel();
        }

        private void OnEnable()
        {
            levelManager.LevelComplete += (level, b) => ToggleLevelComplete(b);
        }

        private void OnDisable()
        {
            levelManager.LevelComplete -= (level, b) => ToggleLevelComplete(b);
        }

        public void LoadLevel()
        {
            curtain.gameObject.SetActive(true);
            startLevelButton.gameObject.SetActive(true);
            winPopUp.Toggle(false);
            losePopUp.Toggle(false);
            interactiveController.SetInteractive(false);
        }

        public void StartLevel()
        {
            curtain.gameObject.SetActive(false);
            startLevelButton.gameObject.SetActive(false);
            interactiveController.SetInteractive(true);
            levelManager.StartLevel();
        }
        
        public void NextLevel()
        {
            levelManager.NextLevel();
            
            LoadLevel();
        }
        
        public void RestartLevel()
        {
            levelManager.ReloadLevel();
            
            LoadLevel();
        }
        
        public void ExitLevel() { }

        public void ToggleLevelComplete(bool complete)
        {
            curtain.gameObject.SetActive(true);
            if (complete)
            {
                winPopUp.Toggle(true);
            }
            else
            {
                losePopUp.Toggle(true);
            }
        }
    }
}