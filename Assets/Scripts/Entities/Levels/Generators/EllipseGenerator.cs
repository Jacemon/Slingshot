using System;
using Managers;
using UnityEngine;

namespace Entities.Levels.Generators
{
    [Serializable]
    public class EllipseGenerator : BaseGenerator
    {
        [Header("Ellipse settings")]
        public Vector2 centerPoint;
        public float spawnRadius;
        public float spawnSecondRadius;
        [Space]
        public int targetsAmount;
        public float spaceBetween;
        
        protected override void StartGenerate()
        {
            var scale = parent != null ? parent.localScale : new Vector3(1, 1, 1);
            generatedTargets = TargetManager.SpawnTargetsByEllipse(
                parent != null ? parent.TransformPoint(centerPoint) : centerPoint,
                spawnRadius * scale.x,
                spawnSecondRadius * scale.y,
                targetsAmount,
                spaceBetween * scale.x,
                randomTargets,
                level,
                parent,
                minMaxScale
            );
        }

#if UNITY_EDITOR
        public override void DrawGizmos()
        {
            Gizmos.color = Color.green;

            var scale = parent != null ? parent.localScale : new Vector3(1, 1, 1);
            Vector2 yUp, yDown, xLeft, xRight;
            
            yUp = yDown = xLeft = xRight = parent != null ? parent.TransformPoint(centerPoint) : centerPoint;
            yUp.y += spawnSecondRadius * scale.y;
            yDown.y -= spawnSecondRadius * scale.y;
            xRight.x += spawnRadius * scale.x;
            xLeft.x -= spawnRadius * scale.x;
            
            Gizmos.DrawLine(yUp, yDown);
            Gizmos.DrawLine(xRight, xLeft);
        }
#endif
    }
}