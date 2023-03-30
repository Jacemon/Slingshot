using System;
using System.Collections.Generic;
using Entities.Targets;
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
        
        private static int _maxTries = 200;
        
        private void Awake()
        {
            _maxTries = maxTriesToSpawn;
        }

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
                () => GetRandomRectanglePoints(rectangle, amount, spaceBetween, _maxTries),
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
                () => GetRandomEllipsePoints(center, semiMinor, semiMajor, amount, spaceBetween, _maxTries),
                targets,
                targetLevel,
                parent,
                minMaxScale);
        }
    }
}