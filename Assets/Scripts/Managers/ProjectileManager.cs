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
        public Vector2 spawnPoint;
        [Space] 
        public int projectileLevel;
        [Space] 
        public TextMeshProUGUI levelLabel;
        public Animator bundleAnimator;
        
        private Projectile _spawnedProjectile;
        private readonly List<Projectile> _thrownProjectiles = new();

        private static readonly int IsFilled = Animator.StringToHash("IsFilled");

        private void Awake()
        {
            GlobalEventManager.UnityEvents.OnLoad.AddListener(LoadData);
            GlobalEventManager.UnityEvents.OnSave.AddListener(SaveData);
            
            SpawnRock();
        }

        private void OnEnable()
        {
            GlobalEventManager.onProjectileLevelUp += LevelUp;
            
            GlobalEventManager.onProjectileThrow += ProjectileThrown;
            GlobalEventManager.onLevelLoad += DeleteThrownProjectiles;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.onProjectileLevelUp -= LevelUp;
            
            GlobalEventManager.onProjectileThrow -= ProjectileThrown;
            GlobalEventManager.onLevelLoad -= DeleteThrownProjectiles;
        }

        private void Update()
        {
            bundleAnimator.SetBool(IsFilled, !_spawnedProjectile.inPick);
        }

        private int LevelUp(int levelCount)
        {
            projectileLevel += levelCount;
            RespawnProjectile();
            
            Debug.Log($"Projectile level: {projectileLevel - levelCount} -> {projectileLevel}");
            
            return projectileLevel;
        }
        
        private void ProjectileThrown(Projectile projectile)
        {
            SpawnProjectile(projectile.projectileName);

            _thrownProjectiles.Add(projectile);
            
            Debug.Log($"{projectile.name} was thrown");
        }

        private void RespawnProjectile()
        {
            var projectileName = _spawnedProjectile.projectileName;
            Destroy(_spawnedProjectile.gameObject);
            SpawnProjectile(projectileName);
        }
        
        private void SpawnProjectile(string projectileName)
        {
            var projectile = projectilePrefabs.Find(o => 
                o.TryGetComponent(out Projectile projectile) 
                && projectile.name == projectileName
            ).GetComponent<Projectile>();
            
            projectile.level = projectileLevel;
            projectile.GetComponent<Follower>().followPoint = spawnPoint;
            _spawnedProjectile = Instantiate(projectile, spawnPoint, Quaternion.identity);
                
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
            
            Debug.Log($"ProjectileManager was loaded: {projectileLevel}");
        }
    }
}
