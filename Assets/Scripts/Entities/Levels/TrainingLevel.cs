using System.Collections.Generic;
using Entities.Levels.Generators;
using Entities.Targets;
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
            _apple.OnHealthChanged += ShowTrainingHint;
            GlobalEventManager.OnTargetHitCart += ShowTrainingGoodEnding;
            GlobalEventManager.OnTargetHitGround += ShowTrainingBadEnding;
        }

        private void OnDisable()
        {
            _apple.OnHealthChanged -= ShowTrainingHint;
            GlobalEventManager.OnTargetHitCart -= ShowTrainingGoodEnding;
            GlobalEventManager.OnTargetHitGround -= ShowTrainingBadEnding;
        }
        
        public override void Generate()
        {
            var pointGenerator = new PointGenerator
            {
                parent = transform,
                points = new List<Vector2> { spawnPoint },
                randomTargets = new List<GameObject> { apple }
            };
            generators.Add(pointGenerator);
            generators[0].Generate();
            
            _apple = ((PointGenerator)generators[0]).generatedTargets[0];
        }
        
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

        private void ShowTrainingHint()
        {
            switch (_apple.health)
            {
                case 1: case 2:
                    ToggleLabel(_apple.health);
                    break;
                default: 
                    ToggleLabel(-1);
                    break;
            }
        }

        private void ShowTrainingBadEnding(Target target)
        {
            _apple = ((PointGenerator)generators[0]).generatedTargets[0];
            ToggleLabel(3);
        }
    
        private void ShowTrainingGoodEnding(Target target)
        {
            _apple = ((PointGenerator)generators[0]).generatedTargets[0];
            ToggleLabel(4);
        }
    }
}