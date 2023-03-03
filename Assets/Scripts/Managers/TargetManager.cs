using System.Collections.Generic;
using Entities;
using UnityEngine;

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

        public static Target SpawnTarget(GameObject[] targets, int targetLevel,
            Vector2 spawnPoint, Transform parent = null)
        {
            if (targets[Random.Range(0, targets.Length)].TryGetComponent(out Target target))
            {
                target.level = targetLevel;
            }
            
            return Instantiate(target, spawnPoint, Quaternion.identity, parent); 
        }
        
        public static List<Target> GenerateTargetsByCircle(GameObject[] targets, int amount, int targetLevel,
            Vector2 spawnPoint, float radius, float spaceBetween, Transform parent = null)
        {
            return GenerateTargetsByEllipse(targets, amount, targetLevel, spawnPoint, 
                radius, radius, spaceBetween, parent);
        }
        
        public static List<Target> GenerateTargetsByEllipse(GameObject[] targets, int amount, int targetLevel,
            Vector2 spawnPoint, float semiMinor, float semiMajor, float spaceBetween, Transform parent = null)
        {
            // Coords calculating
            List<Vector2> existingCoordinates = new();
            var remainingAmount = amount;
            var remainingTries = _maxTries;
            
            while (remainingAmount > 0 && remainingTries > 0) {
                var t = Random.Range(0f, Mathf.PI * 2f);
                var x = Random.Range(0f, semiMinor);
                var y = Random.Range(0f, semiMajor);
                var newCoordinate = new Vector2(x * Mathf.Cos(t), y * Mathf.Sin(t)) + spawnPoint;

                remainingTries--;
                if (!existingCoordinates.TrueForAll(c => Vector2.Distance(c, newCoordinate) > spaceBetween))
                {
                    continue;
                }

                remainingTries = _maxTries;
                remainingAmount--;
                existingCoordinates.Add(newCoordinate);
            }

            List<Target> generatedTargets = new ();
            // Targets instantiating
            existingCoordinates.ForEach(coordinate =>
            {
                generatedTargets.Add(SpawnTarget(targets, targetLevel, coordinate, parent));
            });
            return generatedTargets;
        }
    }
}