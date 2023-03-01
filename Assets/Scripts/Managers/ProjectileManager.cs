using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using TMPro;
using Tools;
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
        
        private static readonly int IsFilled = Animator.StringToHash("IsFilled");
        
        private Vector2 _spawnPoint;
        private readonly Dictionary<string, GameObject> _registeredProjectilePrefabs = new();

        private readonly List<Projectile> _thrownProjectiles = new();

        private Projectile _spawnedProjectile;

        public void Awake()
        {
            GlobalEventManager.OnProjectileThrown.AddListener(ProjectileThrown);
            GlobalEventManager.OnLevelSwitched.AddListener(DeleteThrownProjectiles);
            GlobalEventManager.OnProjectileLevelUpped.AddListener(RespawnProjectile);

            GlobalEventManager.OnSave.AddListener(Save);
            GlobalEventManager.OnLoad.AddListener(Load);
        
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

        private void Update()
        {
            bundleAnimator.SetBool(IsFilled, _spawnedProjectile.state == Projectile.State.InCalm);
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
            _spawnedProjectile = Instantiate(_registeredProjectilePrefabs[projectileName], 
                _spawnPoint, Quaternion.identity).GetComponent<Projectile>();
            
            _spawnedProjectile.GetComponent<Follower>().followPoint = _spawnPoint;
            _spawnedProjectile.level = projectileLevel;
            _spawnedProjectile.Reload();
            
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

        public void Reload()
        {
            RespawnProjectile();
        }
        
        public void Save()
        {
            PlayerPrefs.SetInt("projectileLevel", projectileLevel);
        }

        public void Load()
        {
            projectileLevel = PlayerPrefs.GetInt("projectileLevel");
            Reload();
            
            Debug.Log("ProjectileManager was loaded");
        }
    }
}
