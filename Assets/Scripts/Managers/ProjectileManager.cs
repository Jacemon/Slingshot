using UnityEngine;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> projectilePrefabs = new();
    public GameObject projectileSpawnPoint;
    [Space]
    public Timer timer;

    private Vector2 _spawnPoint;
    private static readonly Dictionary<string, GameObject> RegisteredProjectilePrefabs = new();

    public void Awake()
    {
        GlobalEventManager.OnProjectileThrown.AddListener(ProjectileThrown);
        GlobalEventManager.OnProjectileSpawned.AddListener(ProjectileSpawned);
        
        // Проверка префаба на то, что он снаряд
        foreach (var projectilePrefab in projectilePrefabs)
        {
            var projectile = projectilePrefab.GetComponent<Projectile>();
            if (projectile != null)
            {
                // Добавление снаряда в список
                RegisteredProjectilePrefabs[projectile.projectileName] = projectilePrefab;
                Debug.Log("Projectile prefab " + projectile.projectileName + " was registered");
            }
            else
            {
                Debug.LogError("GameObject " + projectilePrefab.name + " is not Projectile");
            }
        }
        
        _spawnPoint = projectileSpawnPoint.transform.position;
    }

    private void ProjectileThrown(Projectile projectile)
    {
        float extraDelay = 1.0f;

        // Установка большей задержи для таймера
        timer.SetBiggerDelay(projectile.flightTime + extraDelay);
        timer.timerOn = true;
        Debug.Log($"Try set timer to {projectile.flightTime + extraDelay}s");
    }

    private void ProjectileSpawned(Projectile projectile)
    {
        Debug.Log($"{projectile.name} was spawned");
    }

    private void SpawnProjectile(string projectileName)
    {
        var projectile = Instantiate(RegisteredProjectilePrefabs[projectileName], 
            _spawnPoint, Quaternion.identity);
        projectile.GetComponent<Follower>().followPoint = _spawnPoint;
    }
    
    public void SpawnRock() => SpawnProjectile("Rock");
}
