using System;
using Managers;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Entities.Levels.Generators.DynamicGenerators
{
    [Serializable]
    public class DynamicEllipseGenerator : EllipseGenerator
    {
        [Header("Dynamic settings")]
        public MinMaxCurve pointsCount = new(2);
        
        protected override void Generate()
        {
            var scale = parent != null ? parent.localScale : new Vector3(1, 1, 1);
            generatedTargets.AddRange(TargetManager.SpawnDynamicTargetsByEllipse(
                parent != null ? parent.TransformPoint(centerPoint) : centerPoint,
                spawnRadius * scale.x,
                spawnSecondRadius * scale.y,
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