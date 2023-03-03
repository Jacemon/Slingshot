using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using TMPro;
using Tools;
using Tools.Interfaces;
using UnityEngine;

namespace Managers
{
    public class ProjectileManager : MonoBehaviour, ISavable
    {
        [Header("Settings")]
        public List<GameObject> projectilePrefabs = new();
        public GameObject projectileSpawnPoint;
        [Space] 
        public int projectileLevel;
        [Space]
        public Timer timer;
        [Space] 
        public TextMeshProUGUI levelLabel;
        public Animator bundleAnimator;
        
        private readonly Dictionary<string, Projectile> _registeredProjectilePrefabs = new();

        private Vector2 _spawnPoint;
        private Projectile _spawnedProjectile;
        private readonly List<Projectile> _thrownProjectiles = new();

        private static readonly int IsFilled = Animator.StringToHash("IsFilled");
        
        public void Awake()
        {
            GlobalEventManager.OnProjectileThrow.AddListener(ProjectileThrown);
            GlobalEventManager.OnLevelLoad.AddListener(DeleteThrownProjectiles);
            GlobalEventManager.OnProjectileLevelUp.AddListener(LevelUp);

            GlobalEventManager.OnSave.AddListener(SaveData);
            GlobalEventManager.OnLoad.AddListener(LoadData);
        
            // Проверка префаба на то, что он снаряд
            foreach (var projectilePrefab in projectilePrefabs)
            {
                if (projectilePrefab.TryGetComponent(out Projectile projectile))
                {
                    // Добавление снаряда в список
                    _registeredProjectilePrefabs[projectile.projectileName] = projectile;
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

        private void Update()
        {
            bundleAnimator.SetBool(IsFilled, _spawnedProjectile.state == Projectile.State.InCalm);
        }

        private void LevelUp()
        {
            projectileLevel++;
            RespawnProjectile();
        }
        
        private void ProjectileThrown(Projectile projectile)
        {
            const float extraDelay = 1.0f;
            // Установка большей задержи для таймера
            timer.SetBiggerDelay(projectile.flightTime + extraDelay);
            timer.timerOn = true;
            Debug.Log($"Try set timer to {projectile.flightTime + extraDelay}s");
            
            SpawnProjectile(projectile.projectileName);

            _thrownProjectiles.Add(projectile);
            
            Debug.Log($"{projectile.name} was thrown");
        }

        public void RespawnProjectile()
        {
            var projectileName = _spawnedProjectile.projectileName;
            Destroy(_spawnedProjectile.gameObject);
            SpawnProjectile(projectileName);
        }
        
        private void SpawnProjectile(string projectileName)
        {
            var projectile = _registeredProjectilePrefabs[projectileName];
            projectile.level = projectileLevel;
            projectile.GetComponent<Follower>().followPoint = _spawnPoint;
            _spawnedProjectile = Instantiate(projectile, _spawnPoint, Quaternion.identity);
            
            levelLabel.text = projectileLevel.ToString();
        }

        private void DeleteThrownProjectiles()
        {
            foreach (var projectile in _thrownProjectiles.Where(projectile => projectile != null))
            {
                Destroy(projectile.gameObject);
            }

            _thrownProjectiles.Clear();
        }
        
        public void SpawnRock() => SpawnProjectile("Rock");

        public void ReloadData()
        {
            RespawnProjectile();
        }
        
        public void SaveData()
        {
            PlayerPrefs.SetInt("projectileLevel", projectileLevel);
        }

        public void LoadData()
        {
            projectileLevel = PlayerPrefs.GetInt("projectileLevel");
            ReloadData();
            
            Debug.Log("ProjectileManager was loaded");
        }
    }
}
