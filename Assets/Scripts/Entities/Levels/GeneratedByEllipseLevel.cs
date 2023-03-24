using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Entities.Levels
{
    public class GeneratedByEllipseLevel : Level
    {
        [Header("Target generator settings")]
        public GameObject[] targets;
        public int targetsAmount;
        public ParticleSystem.MinMaxCurve minMaxScale = new(1);
        public Vector2 spawnPoint;
        public float spawnRadius;
        public float spawnSecondRadius;
        public float spaceBetween;

        private List<Target> _generatedTargets = new();
    
        private void Awake()
        {
            Generate();
        }

        private void OnEnable()
        {
            GlobalEventManager.onTargetHitCart += CheckTargets;
            GlobalEventManager.onTargetHitGround += CheckTargets;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.onTargetHitCart -= CheckTargets;
            GlobalEventManager.onTargetHitGround -= CheckTargets;
        }

        private void Generate()
        {
            _generatedTargets = TargetManager.GenerateTargetsByEllipse(
                targets, targetsAmount, levelNumber, 
                transform.TransformPoint(spawnPoint), 
                spawnRadius * transform.localScale.x, 
                spawnSecondRadius * transform.localScale.y, 
                spaceBetween * transform.localScale.x,
                transform,
                minMaxScale
            );
        }
        
        private void CheckTargets(Target target)
        {
            _generatedTargets.Remove(target);
            if (_generatedTargets.Count == 0)
            {
                Generate();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Vector2 yUp, yDown, xLeft, xRight;
            yUp = yDown = xLeft = xRight = transform.TransformPoint(spawnPoint);
            yUp.y += spawnSecondRadius * transform.localScale.y;
            yDown.y -= spawnSecondRadius * transform.localScale.y;
            xRight.x += spawnRadius * transform.localScale.x;
            xLeft.x -= spawnRadius * transform.localScale.x;
            Gizmos.DrawLine(yUp, yDown);
            Gizmos.DrawLine(xRight, xLeft);
        }
    }
}
