using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ScoreManager))]
public class ProjectileManager : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> projectilePrefabs = new();
    public Vector3 projectileSpawnPoint;

    private static readonly Dictionary<string, GameObject> ProjectilePrefabsDictionary = new();
    
    private ScoreManager _scoreManager;
    
    public void Awake()
    {
        _scoreManager = GetComponent<ScoreManager>();
        
        // Проверка префаба на то, что он снаряд
        foreach (var projectilePrefab in projectilePrefabs)
        {
            Projectile projectile = projectilePrefab.GetComponent<Projectile>();
            if (projectile != null)
            {
                // Добавление снаряда в список
                ProjectilePrefabsDictionary[projectile.projectileName] = projectilePrefab;
                Debug.Log("Projectile prefab " + projectile.projectileName + " was loaded");
            }
            else
            {
                Debug.LogError("GameObject " + projectilePrefab.name + " is not Projectile");
            }
        }
    }

    private void SpawnProjectile(string projectileName)
    {
        _scoreManager.AddDestroyableGameObject(Instantiate(ProjectilePrefabsDictionary[projectileName], 
            projectileSpawnPoint, Quaternion.identity));
    }

    public void SpawnRock() => SpawnProjectile("Rock");
}
