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
            GlobalEventManager.OnTargetSpawned.AddListener(TargetSpawned);
            _maxTries = maxTriesToSpawn; // todo mb remake
        }

        private void TargetSpawned(Target target)
        {
            Debug.Log($"{target.name} was spawned");
        }

        public static void SpawnTarget(GameObject[] targets, int targetLevel,
            Vector2 spawnPoint, Transform parent = null)
        {
            var gameObject = Instantiate(targets[Random.Range(0, targets.Length)],
                spawnPoint, Quaternion.identity);
            gameObject.transform.SetParent(parent);
            if (gameObject.TryGetComponent(out Target target))
            {
                target.level = targetLevel;
                target.Reload();
            }
        }
        
        public static void GenerateTargetsByCircle(GameObject[] targets, int amount, int targetLevel,
            Vector2 spawnPoint, float radius, float spaceBetween, Transform parent = null)
        {
            GenerateTargetsByEllipse(targets, amount, targetLevel, spawnPoint, radius, radius, spaceBetween, parent);
        }
        
        public static void GenerateTargetsByEllipse(GameObject[] targets, int amount, int targetLevel,
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
        
            // Targets instantiating
            existingCoordinates.ForEach(coordinate =>
            {
                SpawnTarget(targets, targetLevel, coordinate, parent);
            });
        }
    }
}