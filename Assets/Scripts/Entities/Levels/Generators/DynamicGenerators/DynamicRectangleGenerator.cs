using System;
using Managers;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Entities.Levels.Generators.DynamicGenerators
{
    [Serializable]
    public class DynamicRectangleGenerator : RectangleGenerator
    {
        [Header("Dynamic settings")]
        public MinMaxCurve pointsCount = new(2);
        
        protected override void Generate()
        {
            var scale = parent != null ? parent.localScale : new Vector3(1, 1, 1);
            var localRectangle = parent != null ? new Rect(
                parent.TransformPoint(rectangle.x, rectangle.y, 0),
                new Vector2(rectangle.width * scale.x, rectangle.height * scale.y)
            ) : rectangle;
            generatedTargets.AddRange(TargetManager.SpawnDynamicTargetsByRectangle(
                localRectangle,
                targetsAmount,
                pointsCount,
                spaceBetween * scale.x,
                randomTargets, 
                level, 
                parent,
                minMaxScale
            ));
        }
    }
}