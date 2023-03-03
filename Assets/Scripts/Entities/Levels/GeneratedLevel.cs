using System.Collections.Generic;
using System.Linq;
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

        private List<Target> _generatedTargets = new();
    
        private void Awake()
        {
            foreach (var target in _generatedTargets.Where(target => target != null))
            {
                Destroy(target.gameObject);
            }
            _generatedTargets.Clear();
            _generatedTargets = TargetManager.GenerateTargetsByEllipse(targets, targetsAmount, levelNumber, 
                spawnPoint, spawnRadius, spawnSecondRadius, spaceBetween, transform);
            foreach (var target in _generatedTargets)
            {
                target.level = levelNumber;
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
