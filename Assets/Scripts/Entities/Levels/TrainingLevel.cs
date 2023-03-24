using Managers;
using TMPro;
using UnityEngine;

namespace Entities.Levels
{
    public class TrainingLevel : Level
    {
        public GameObject hand;
        public GameObject apple;
        public Vector2 spawnPoint;
        [Space]
        public TextMeshProUGUI[] helpLabels;
        
        private Target _apple;

        private void Awake()
        {
            Generate();
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

        private void Generate()
        {
            _apple = TargetManager.SpawnTarget(new[] { apple }, levelNumber, 
                transform.TransformPoint(spawnPoint), transform, new ParticleSystem.MinMaxCurve(1));
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
            hand.SetActive(_apple.health == _apple.maxHealth);
        }

        private void ShowTrainingHint(Target target)
        {
            if (target != _apple)
            {
                return;
            }

            switch (_apple.health)
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
            Generate();
            ToggleLabel(3);
        }
    
        private void ShowTrainingGoodEnding(Target target)
        {
            Generate();
            ToggleLabel(4);
        }
    }
}
