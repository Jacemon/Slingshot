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
        private static int maxTries = 200;
        [Header("Spawn settings")]
        public int maxTriesToSpawn;

        private void Awake()
        {
            maxTries = maxTriesToSpawn;
        }

        #region Static

        public static Target SpawnTarget(
            Vector2 spawnPoint,
            List<Target> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            var target = targets[Random.Range(0, targets.Count)];
            target.level = targetLevel;
            target.appearScale = minMaxScale.Evaluate(Time.time, Random.Range(0.0f, 1.0f));

            var spawnedTarget = Instantiate(target, spawnPoint, Quaternion.identity, parent);
            var position = spawnedTarget.transform.localPosition; // TODO: fix local position in generators or this(??)
            position.z = target.transform.position.z; // todo and here
            spawnedTarget.transform.localPosition = position; // todo here too

            return spawnedTarget;
        }

        public static List<Target> SpawnTargetsByFunction(
            Func<List<Vector2>> generationFunction,
            List<Target> targets,
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
            List<Target> targets,
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
            List<Target> targets,
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
            List<Target> targets,
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

        public static Target SpawnDynamicTarget(
            List<Vector2> path,
            List<Target> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            if (targets[Random.Range(0, targets.Count)].TryGetComponent(out DynamicTarget target))
            {
                target.level = targetLevel;
                target.appearScale = minMaxScale.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
                target.GetComponent<PathFollower>().vector2Points = path;
            }

            return Instantiate(target, parent);
        }

        public static List<Target> SpawnDynamicTargetsByFunction(
            Func<List<Vector2>> generationFunction,
            List<Target> targets,
            int targetLevel,
            int amount,
            Transform parent = null,
            MinMaxCurve minMaxScale = default)
        {
            var dynamicTargets = new List<Target>();
            for (var i = 0; i < amount; i++)
            {
                var path = generationFunction.Invoke();
                dynamicTargets.Add(SpawnDynamicTarget(path, targets, targetLevel, parent, minMaxScale));
            }

            return dynamicTargets;
        }

        public static List<Target> SpawnDynamicTargetsByPoints(
            List<Vector2> points,
            int amount,
            List<Target> targets,
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

        public static List<Target> SpawnDynamicTargetsByRectangle(
            Rect rectangle,
            int amount,
            MinMaxCurve pointsAmount,
            float spaceBetween,
            List<Target> targets,
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

        public static List<Target> SpawnDynamicTargetsByEllipse(
            Vector2 center,
            float semiMinor,
            float semiMajor,
            int amount,
            MinMaxCurve pointsAmount,
            float spaceBetween,
            List<Target> targets,
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