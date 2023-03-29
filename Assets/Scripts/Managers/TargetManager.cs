using System.Collections.Generic;
using Entities.Targets;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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

        public static List<Target> GenerateTargetsByRectangle(
            Rect rectangle, 
            int amount,
            float spaceBetween,
            List<GameObject> targets,
            int targetLevel,
            Transform parent = null,
            MinMaxCurve minMaxScale = default
        )
        {
            List<Vector2> existingCoordinates = new();
            var remainingAmount = amount;
            var remainingTries = _maxTries;
            
            while (remainingAmount > 0 && remainingTries > 0) {
                var newCoordinate = new Vector2(
                    Random.Range(rectangle.x, rectangle.x + rectangle.width),
                    Random.Range(rectangle.y, rectangle.y + rectangle.height)
                );
                
                remainingTries--;
                if (!existingCoordinates.TrueForAll(c => Vector2.Distance(c, newCoordinate) > spaceBetween))
                {
                    continue;
                }

                remainingTries = _maxTries;
                remainingAmount--;
                existingCoordinates.Add(newCoordinate);
            }

            // Targets instantiating
            List<Target> generatedTargets = new();
            existingCoordinates.ForEach(coordinate =>
            {
                generatedTargets.Add(SpawnTarget(coordinate, targets, targetLevel, parent, minMaxScale));
            });
            return generatedTargets;
        }

        public static List<Target> GenerateTargetsByEllipse(
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
            // Coords calculating
            List<Vector2> existingCoordinates = new();
            var remainingAmount = amount;
            var remainingTries = _maxTries;
            
            while (remainingAmount > 0 && remainingTries > 0) {
                var t = Random.Range(0f, Mathf.PI * 2f);
                var x = Random.Range(0f, semiMinor);
                var y = Random.Range(0f, semiMajor);
                var newCoordinate = new Vector2(x * Mathf.Cos(t), y * Mathf.Sin(t)) + center;

                remainingTries--;
                if (!existingCoordinates.TrueForAll(c => Vector2.Distance(c, newCoordinate) > spaceBetween))
                {
                    continue;
                }

                remainingTries = _maxTries;
                remainingAmount--;
                existingCoordinates.Add(newCoordinate);
            }

            // Targets instantiating
            List<Target> generatedTargets = new();
            existingCoordinates.ForEach(coordinate =>
            {
                generatedTargets.Add(SpawnTarget(coordinate, targets, targetLevel, parent, minMaxScale));
            });
            return generatedTargets;
        }
    }
}