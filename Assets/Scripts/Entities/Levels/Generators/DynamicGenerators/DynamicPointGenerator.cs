using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Entities.Levels.Generators.DynamicGenerators
{
    [Serializable]
    public class DynamicPointGenerator : PointGenerator
    {
        [Header("Dynamic settings")]
        public int amount = 1;

        protected override void Generate()
        {
            List<Vector2> globalPoints = new();

            if (parent != null)
                foreach (var point in points)
                    globalPoints.Add(parent.TransformPoint(point));
            else
                globalPoints = points;

            generatedTargets.AddRange(TargetManager.SpawnDynamicTargetsByPoints(
                globalPoints,
                amount,
                randomTargets,
                level,
                parent,
                minMaxScale
            ));
        }
    }
}