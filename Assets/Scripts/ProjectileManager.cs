using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileManager : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> projectilePrefabs = new List<GameObject>();

    public Vector3 projectileSpawnPoint;

    private static Dictionary<string, GameObject> _projectileDictionary = new Dictionary<string, GameObject>();

    // Переделать
    public void Start()
    {
        // Проверка префаба на то, что он снаряд
        foreach (var projectilePrefab in projectilePrefabs)
        {
            Projectile projectile = projectilePrefab.GetComponent<Projectile>();
            if (projectile != null)
            {
                // Добавление снаряда в список
                _projectileDictionary[projectile.projectileName] = projectilePrefab;
                Debug.Log("Projectile " + projectile.projectileName + " was loaded");
            }
            else
            {
                Debug.LogError("GameObject " + projectilePrefab.name + " is not Projectile");
            }
        }
    }

    public static GameObject SpawnProjectile(string projectileName)
    {
        return Instantiate(_projectileDictionary[projectileName]);
    }
}
