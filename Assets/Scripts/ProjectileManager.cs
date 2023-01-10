using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[RequireComponent(typeof(ScoreManager))]
public class ProjectileManager : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> projectilePrefabs = new();
    public Vector3 projectileSpawnPoint;
    public Timer timer;

    private static readonly Dictionary<string, GameObject> RegisteredProjectilePrefabs = new();
    
    private ScoreManager _scoreManager;
    
    public void Awake()
    {
        _scoreManager = GetComponent<ScoreManager>();
        
        // Регистрация уже добавленных на сцене снарядов
        foreach (var projectileGameObject in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            _scoreManager.AddDestroyableGameObject(projectileGameObject);
            
            AddFlightEvent(projectileGameObject);
        }
        
        // Проверка префаба на то, что он снаряд
        foreach (var projectilePrefab in projectilePrefabs)
        {
            var projectile = projectilePrefab.GetComponent<Projectile>();
            if (projectile != null)
            {
                // Добавление снаряда в список
                RegisteredProjectilePrefabs[projectile.projectileName] = projectilePrefab;
                Debug.Log("Projectile prefab " + projectile.projectileName + " was loaded");
            }
            else
            {
                Debug.LogError("GameObject " + projectilePrefab.name + " is not Projectile");
            }
        }
    }

    private void AddFlightEvent(GameObject projectileGameObject)
    {
        float extraDelay = 1.0f;
        // Добавления события при полёте (установка большей задержи для таймера)
        var projectile = projectileGameObject.GetComponent<Projectile>();
        projectile.OnFlight += () =>
        {
            timer.SetBiggerDelay(projectile.flightTime + extraDelay);
            timer.timerOn = true;
            Debug.Log($"Try set timer to {projectile.flightTime + extraDelay}s");
        };
    }
    
    private void SpawnProjectile(string projectileName)
    {
        if (_scoreManager.GetProjectile() == 0)
        {
            return;
        }
        
        var projectileGameObject = Instantiate(RegisteredProjectilePrefabs[projectileName], 
            projectileSpawnPoint, Quaternion.identity);
        _scoreManager.AddDestroyableGameObject(projectileGameObject);
        
        AddFlightEvent(projectileGameObject);
    }

    public void SpawnRock() => SpawnProjectile("Rock");
}
