using Managers;
using TMPro;
using UnityEngine;

namespace Entities.Levels
{
    public class TrainingLevel : Level
    {
        public GameObject hand;
        public Target apple;
        [Space]
        public TextMeshProUGUI[] helpLabels;
    
        private Animator _handAnimator;

        private void Awake()
        {
            _handAnimator = hand.GetComponent<Animator>();
        
            GlobalEventManager.OnTargetGetDamage.AddListener(ShowTrainingHint);
            GlobalEventManager.OnTargetHitCart.AddListener(ShowTrainingGoodEnding);
            GlobalEventManager.OnTargetHitGround.AddListener(ShowTrainingBadEnding);
        
            ToggleLabel(0);
        }

        // if i < -1 disable all
        private void ToggleLabel(int i)
        {
            foreach (var label in helpLabels)
            {
                label.enabled = false;
            }

            if (i >= 0)
            {
                helpLabels[i].enabled = true;
            }
        }

        private void Update()
        {
            hand.SetActive(apple.health == apple.maxHealth);
        }

        private void ShowTrainingHint(Target target)
        {
            if (target != apple)
            {
                return;
            }

            switch (apple.health)
            {
                case 1: 
                    ToggleLabel(1);
                    break;
                case 2:
                    ToggleLabel(2);
                    break;
                default: 
                    ToggleLabel(-1);
                    break;
            }
        }

        private void ShowTrainingBadEnding(Target target)
        {
            if (target != apple)
            {
                return;
            }
            ToggleLabel(3);
        }
    
        private void ShowTrainingGoodEnding(Target target)
        {
            if (target != apple)
            {
                return;
            }
            ToggleLabel(4);
        }
    }
}
