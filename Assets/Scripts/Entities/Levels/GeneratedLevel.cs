using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Entities.Levels
{
    public class GeneratedLevel : Level
    {
        [Header("Target generator settings")]
        public GameObject[] targets;
        public int targetsAmount;
        public Vector2 spawnPoint;
        public float spawnRadius;
        public float spawnSecondRadius;
        public float spaceBetween;

        private List<Target> _generatedTargets;
    
        private void Awake()
        {
            _generatedTargets = TargetManager.GenerateTargetsByEllipse(targets, targetsAmount, levelNumber, 
                spawnPoint, spawnRadius, spawnSecondRadius, spaceBetween, transform);
        }

        public override void Reload()
        {
            foreach (var target in _generatedTargets)
            {
                target.level = levelNumber;
                target.Reload();
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
