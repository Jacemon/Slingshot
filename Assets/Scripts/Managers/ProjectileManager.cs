using System.Collections.Generic;
using Entities;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using Projectile = Entities.Projectile;

namespace Managers
{
    public class ProjectileManager : MonoBehaviour
    {
        [Header("Settings")]
        public List<GameObject> projectilePrefabs = new();
        public GameObject projectileSpawnPoint;
        [Space]
        public Timer timer;
        
        private Vector2 _spawnPoint;
        private readonly Dictionary<string, GameObject> _registeredProjectilePrefabs = new();

        public void Awake()
        {
            GlobalEventManager.OnProjectileThrown.AddListener(ProjectileThrown);
            GlobalEventManager.OnProjectileSpawned.AddListener(ProjectileSpawned);
            GlobalEventManager.OnLevelSwitched.AddListener(DeleteThrownProjectiles);
        
            // Проверка префаба на то, что он снаряд
            foreach (var projectilePrefab in projectilePrefabs)
            {
                var projectile = projectilePrefab.GetComponent<Projectile>();
                if (projectile != null)
                {
                    // Добавление снаряда в список
                    _registeredProjectilePrefabs[projectile.projectileName] = projectilePrefab;
                    Debug.Log("Projectile prefab " + projectile.projectileName + " was registered");
                }
                else
                {
                    Debug.LogError("GameObject " + projectilePrefab.name + " is not Projectile");
                }
            }
        
            _spawnPoint = projectileSpawnPoint.transform.position;
            
            SpawnRock();
        }

        private void ProjectileThrown(Projectile projectile)
        {
            const float extraDelay = 1.0f;
            // Установка большей задержи для таймера
            timer.SetBiggerDelay(projectile.flightTime + extraDelay);
            timer.timerOn = true;
            Debug.Log($"Try set timer to {projectile.flightTime + extraDelay}s");
            
            SpawnProjectile(projectile.projectileName);

            Debug.Log($"{projectile.name} was thrown");
        }

        private void ProjectileSpawned(Projectile projectile)
        {
            projectile.GetComponent<Follower>().followPoint = _spawnPoint;
            Debug.Log($"{projectile.name} was spawned");
        }

        private void SpawnProjectile(string projectileName)
        {
            Instantiate(_registeredProjectilePrefabs[projectileName], _spawnPoint, Quaternion.identity);
        }

        private void DeleteThrownProjectiles()
        {
            var projectileObjects = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (var projectileObject in projectileObjects)
            {
                var projectile = projectileObject.GetComponent<Projectile>();
                if (projectile != null && projectile.state is Projectile.State.InFlight or Projectile.State.InHit)
                {
                    Destroy(projectileObject);
                }
            }
        }
        
        public void SpawnRock() => SpawnProjectile("Rock");
    }
}
