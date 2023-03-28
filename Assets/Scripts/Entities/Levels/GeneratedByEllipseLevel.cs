using System.Collections.Generic;
using Entities.Targets;
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
            var scale = transform.localScale;
            _generatedTargets = TargetManager.GenerateTargetsByEllipse(
                targets, targetsAmount, levelNumber, 
                transform.TransformPoint(spawnPoint), 
                spawnRadius * scale.x, 
                spawnSecondRadius * scale.y, 
                spaceBetween * scale.x,
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

            var scale = transform.localScale;
            Vector2 yUp, yDown, xLeft, xRight;
            yUp = yDown = xLeft = xRight = transform.TransformPoint(spawnPoint);
            yUp.y += spawnSecondRadius * scale.y;
            yDown.y -= spawnSecondRadius * scale.y;
            xRight.x += spawnRadius * scale.x;
            xLeft.x -= spawnRadius * scale.x;
            Gizmos.DrawLine(yUp, yDown);
            Gizmos.DrawLine(xRight, xLeft);
        }
    }
}
