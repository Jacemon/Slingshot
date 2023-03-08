using System;
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

        private void Awake()
        {
            ToggleLabel(0);
        }

        private void OnEnable()
        {
            GlobalEventManager.onTargetGetDamage += ShowTrainingHint;
            GlobalEventManager.onTargetHitCart += ShowTrainingGoodEnding;
            GlobalEventManager.onTargetHitGround += ShowTrainingBadEnding;
        }

        private void OnDisable()
        {
            GlobalEventManager.onTargetGetDamage -= ShowTrainingHint;
            GlobalEventManager.onTargetHitCart -= ShowTrainingGoodEnding;
            GlobalEventManager.onTargetHitGround -= ShowTrainingBadEnding;
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
