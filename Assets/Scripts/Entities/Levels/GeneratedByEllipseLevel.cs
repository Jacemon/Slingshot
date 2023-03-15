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
            _generatedTargets = TargetManager.GenerateTargetsByEllipse(targets, targetsAmount, levelNumber, 
                spawnPoint, spawnRadius, spawnSecondRadius, spaceBetween, transform);
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
            yUp = yDown = xLeft = xRight = spawnPoint;
            yUp.y += spawnSecondRadius;
            yDown.y -= spawnSecondRadius;
            xRight.x += spawnRadius;
            xLeft.x -= spawnRadius;
            Gizmos.DrawLine(yUp, yDown);
            Gizmos.DrawLine(xRight, xLeft);
        }
    }
}
