using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Serialization;

public class ProjectileManager : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> projectilePrefabs = new();
    public Vector3 projectileSpawnPoint;
    public Timer timer;

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
        Instantiate(RegisteredProjectilePrefabs[projectileName], projectileSpawnPoint, Quaternion.identity);
    }
    
    public void SpawnRock() => SpawnProjectile("Rock");
}
