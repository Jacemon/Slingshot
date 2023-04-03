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
        public List<Vector2> points = new();

        protected override void StartGenerate()
        {
            List<Vector2> globalPoints = new();
            
            if (parent != null)
            {
                foreach (var point in points)
                {
                    globalPoints.Add(parent.TransformPoint(point));
                }
            }
            else
            {
                globalPoints = points;
            }
            
            generatedTargets = TargetManager.SpawnTargetsByPoints(
                globalPoints,
                randomTargets,
                level,
                parent, 
                minMaxScale
            );
        }
        
#if UNITY_EDITOR
        public override void DrawGizmos()
        {
            foreach (var point in points)
            {
                Handles.color = Color.green;
                Handles.DrawWireDisc(
                    parent != null ? parent.TransformPoint(point) : point,
                    Vector3.forward,
                    parent.localScale.x
                );
            }
        }
#endif
    }
}