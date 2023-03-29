using System;
using System.Collections.Generic;
using Managers;
using UnityEditor;
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
                        parent != null ? parent.TransformPoint(point.point) : point.point,
                        point.target == null ? randomTargets : new List<GameObject> { point.target },
                        level,
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
                Handles.color = point.target == null ? Color.yellow : Color.green;
                Handles.DrawWireDisc(
                    parent != null ? parent.TransformPoint(point.point) : point.point,
                    Vector3.forward,
                    parent.localScale.x
                );
            }
        }
    }
}