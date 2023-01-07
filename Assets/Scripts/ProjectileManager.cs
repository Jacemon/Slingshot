using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Header("Settigs")]
    public List<GameObject> ProjectilePrefabs = new List<GameObject>();
    public Vector3 ProjectileSpawnPoint = Vector3.zero;

    private static Dictionary<string, GameObject> ProjectileDictionary = new Dictionary<string, GameObject>();

    // Переделать
    public void Start()
    {
        // Проверка префаба на то, что он снаряд
        foreach (var projectilePrefab in ProjectilePrefabs)
        {
            Projectile projectile = projectilePrefab.GetComponent<Projectile>();
            if (projectile != null)
            {
                // Добавление снаряда в список
                ProjectileDictionary[projectile.ProjectileName] = projectilePrefab;
                Debug.Log("Projectile " + projectile.ProjectileName + " was loaded");
            }
            else
            {
                Debug.LogError("GameObject " + projectilePrefab.name + " is not Projectile");
            }
        }
    }

    public static GameObject SpawnProjectile(string projectileName)
    {
        return Instantiate(ProjectileDictionary[projectileName]);
    }
}
