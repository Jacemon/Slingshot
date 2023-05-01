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
        public Target apple; // TODO: баг какой-то тут есть, а может уже и нет, проверю как-нибудь потом
        public Vector2 spawnPoint;
        [Space]
        public TextMeshProUGUI[] helpLabels;

        private bool _isHit; 
            
        private Target _apple;
        private Animator _handAnimator;
        private static readonly int ShowPouchHint = Animator.StringToHash("ShowPouchHint");
        private static readonly int ShowBuyLevelHint = Animator.StringToHash("ShowBuyLevelHint");

        protected override void Awake()
        {
            base.Awake();
            
            _handAnimator = hand.GetComponent<Animator>();
            ToggleLabel(0);
            ShowTrainingHint();
        }
        
        private void OnEnable()
        {
            GlobalEventManager.OnTargetHitCart += ShowTrainingGoodEnding;
            GlobalEventManager.OnTargetHitGround += ShowTrainingBadEnding;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.OnTargetHitCart -= ShowTrainingGoodEnding;
            GlobalEventManager.OnTargetHitGround -= ShowTrainingBadEnding;
        }
        
        public override void StartGenerate()
        {
            var pointGenerator = new PointGenerator
            {
                parent = transform,
                points = new List<Vector2> { spawnPoint },
                randomTargets = new List<Target> { apple }
            };
            generators.Add(pointGenerator);
            generators[0].StartGenerate();
            
            UpdateApple();
        }

        private void UpdateApple()
        {
            _apple = ((PointGenerator)generators[0]).generatedTargets[0];
            _apple.OnHealthChanged += ShowTrainingHint;
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

        private void ShowTrainingHint()
        {
            // if (_apple.health == _apple.maxHealth)
            // {
            //     ToggleLabel(0);
            //     _handAnimator.SetBool(ShowPouchHint, true);
            // }
            // else if (_apple.health <= 0)
            // {
            //     if (_isHit)
            //     {
            //         ToggleLabel(4);
            //         _handAnimator.SetBool(ShowBuyLevelHint, _apple.health <= 0);
            //         _isHit = false;
            //         UpdateApple();
            //     }
            //     else
            //     {
            //         _isHit = true;
            //         ToggleLabel(-1);
            //     }
            // }
            // else
            // {
            //     ToggleLabel(_apple.health);
            // }
            _handAnimator.SetBool(ShowPouchHint, false);
            switch (_apple.health)
            {
                case 1: case 2:
                    ToggleLabel(_apple.health);
                    break;
                case 3:
                    _handAnimator.SetBool(ShowPouchHint, true);
                    break;
                default: 
                    ToggleLabel(-1);
                    break;
            }
        }

        private void ShowTrainingBadEnding(Target target)
        {
            UpdateApple();
            ToggleLabel(3);
        }
    
        private void ShowTrainingGoodEnding(Target target)
        {
            UpdateApple();
            ToggleLabel(4);
        }
    }
}