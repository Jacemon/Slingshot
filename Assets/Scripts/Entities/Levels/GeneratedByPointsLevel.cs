using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Entities.Levels
{
    public class GeneratedByPointsLevel : Level
    {
        [Header("Target generator settings")]
        public List<TargetPointPair> points;
        [Tooltip("These targets are used instead of null targets in the Points list")]
        public GameObject[] auxiliaryTargets;
            
        private List<Target> _generatedTargets = new();
        
        [Serializable]
        public class TargetPointPair
        {
            public GameObject target;
            public Vector2 point;
        }
    
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
            _generatedTargets = new List<Target>();
            foreach (var point in points)
            {
                _generatedTargets.Add(
                    TargetManager.SpawnTarget(point.target == null ? auxiliaryTargets : new [] { point.target }, 
                        levelNumber, point.point, transform));
            }
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
            foreach (var point in points)
            {
                Gizmos.color = point.target == null ? Color.yellow : Color.green;
                Gizmos.DrawWireSphere(point.point, 0.1f);
            }
        }
    }
}
