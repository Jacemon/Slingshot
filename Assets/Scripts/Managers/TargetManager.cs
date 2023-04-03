using System;
using System.Collections.Generic;
using Entities.Targets;
using Tools.Follower;
using UnityEngine;
using static Tools.PointsGenerators;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

namespace Managers
{
    public class TargetManager : MonoBehaviour
    {
        [Header("Spawn settings")]
        public int maxTriesToSpawn;
        
        private static int maxTries = 200;
        
        private void Awake()
        {
            maxTries = maxTriesToSpawn;
        }
        
        #region Static

        public static Target SpawnTarget(
            Vector2 spawnPoint,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            if (targets[Random.Range(0, targets.Count)].TryGetComponent(out Target target))
            {
                target.level = targetLevel;
                target.appearScale = minMaxScale.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
            }
            return Instantiate(target, spawnPoint, Quaternion.identity, parent);
        }
        
        public static List<Target> SpawnTargetsByFunction(
            Func<List<Vector2>> generationFunction,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default)
        {
            var existingCoordinates = generationFunction.Invoke();
            
            List<Target> generatedTargets = new();
            existingCoordinates.ForEach(coordinate =>
            {
                generatedTargets.Add(SpawnTarget(coordinate, targets, targetLevel, parent, minMaxScale));
            });
            return generatedTargets;
        }

        public static List<Target> SpawnTargetsByPoints(
            List<Vector2> points,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            return SpawnTargetsByFunction(
                () => points,
                targets,
                targetLevel,
                parent,
                minMaxScale);
        }
        
        public static List<Target> SpawnTargetsByRectangle(
            Rect rectangle, 
            int amount,
            float spaceBetween,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            return SpawnTargetsByFunction(
                () => GetRandomRectanglePoints(rectangle, amount, spaceBetween, maxTries),
                targets,
                targetLevel,
                parent,
                minMaxScale);
        }

        public static List<Target> SpawnTargetsByEllipse(
            Vector2 center,
            float semiMinor,
            float semiMajor,
            int amount,
            float spaceBetween,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            return SpawnTargetsByFunction(
                () => GetRandomEllipsePoints(center, semiMinor, semiMajor, amount, spaceBetween, maxTries),
                targets,
                targetLevel,
                parent,
                minMaxScale);
        }

        #endregion
        
        #region Dynamic
        
        public static DynamicTarget SpawnDynamicTarget(
            List<Vector2> path,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            if (targets[Random.Range(0, targets.Count)].TryGetComponent(out DynamicTarget target))
            {
                target.level = targetLevel;
                target.appearScale = minMaxScale.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
                target.GetComponent<PathFollower>().points = path;
            }
            return Instantiate(target, parent);
        }
        
        public static List<DynamicTarget> SpawnDynamicTargetsByFunction(
            Func<List<Vector2>> generationFunction,
            List<GameObject> targets,
            int targetLevel,
            int amount,
            Transform parent = null,
            MinMaxCurve minMaxScale = default)
        {
            var dynamicTargets = new List<DynamicTarget>();
            for (var i = 0; i < amount; i++)
            {
                var path = generationFunction.Invoke();
                dynamicTargets.Add(SpawnDynamicTarget(path, targets, targetLevel, parent, minMaxScale));
            }

            return dynamicTargets;
        }
        
        public static List<DynamicTarget> SpawnDynamicTargetsByPoints(
            List<Vector2> points,
            int amount,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            return SpawnDynamicTargetsByFunction(
                () => points,
                targets,
                targetLevel,
                amount,
                parent,
                minMaxScale);
        }
        
        public static List<DynamicTarget> SpawnDynamicTargetsByRectangle(
            Rect rectangle, 
            int amount,
            MinMaxCurve pointsAmount,
            float spaceBetween,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            return SpawnDynamicTargetsByFunction(
                () => GetRandomRectanglePoints(rectangle, 
                    (int)pointsAmount.Evaluate(Time.time, Random.Range(0.0f, 1.0f)), spaceBetween, maxTries),
                targets,
                targetLevel,
                amount,
                parent,
                minMaxScale);
        }
        
        public static List<DynamicTarget> SpawnDynamicTargetsByEllipse(
            Vector2 center,
            float semiMinor,
            float semiMajor,
            int amount,
            MinMaxCurve pointsAmount,
            float spaceBetween,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            return SpawnDynamicTargetsByFunction(
                () => GetRandomEllipsePoints(center, semiMinor, semiMajor, 
                    (int)pointsAmount.Evaluate(Time.time, Random.Range(0.0f, 1.0f)), spaceBetween, maxTries),
                targets,
                targetLevel,
                amount,
                parent,
                minMaxScale);
        }
        
        #endregion
    }
}