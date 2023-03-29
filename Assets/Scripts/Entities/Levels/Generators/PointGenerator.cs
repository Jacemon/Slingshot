using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Entities.Levels.Generators
{
    [Serializable]
    public class PointGenerator : BaseGenerator
    {
        [Header("Points settings")]
        public List<TargetPointPair> points = new();

        [Serializable]
        public class TargetPointPair
        {
            public GameObject target;
            public Vector2 point;
        }
        
        protected override void StartGenerate()
        {
            foreach (var point in points)
            {
                generatedTargets.Add(
                    TargetManager.SpawnTarget(
                        point.target == null ? randomTargets : new List<GameObject> { point.target },
                        level,
                        parent.TransformPoint(point.point),
                        parent, 
                        minMaxScale
                    )
                );
            }
        }
        
        public override void DrawGizmos()
        {
            foreach (var point in points)
            {
                Gizmos.color = point.target == null ? Color.yellow : Color.green;
                Gizmos.DrawWireSphere(parent != null ? parent.TransformPoint(point.point) : point.point, 
                    parent.localScale.x);
            }
        }
    }
}