using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Levels;
using Tools.Follower;
using Tools.Interactive;
using Tools.ScriptableObjects.References;
using UnityEngine;

namespace Managers
{
    public class ProjectileManager : InteractiveGameObject
    {
        [Header("Settings")]
        public List<Projectile> projectilePrefabs = new();
        public Vector2 spawnPoint;
        [Space]
        public IntReference projectileLevel;
        [Space]
        public Animator bundleAnimator;
        
        private readonly List<Projectile> _thrownProjectiles = new();
        private Projectile _spawnedProjectile;
        private bool _interactive = true;
        
        private static readonly int IsFilled = Animator.StringToHash("IsFilled");

        private void Awake()
        {
            SpawnRock();
        }

        private void Update()
        {
            bundleAnimator.SetBool(IsFilled, !_spawnedProjectile.inPick);
        }

        private void OnEnable()
        {
            projectileLevel.OnValueChanged += OnProjectileLevelChanged;

            GlobalEventManager.OnProjectileThrown += ProjectileThrown;
            GlobalEventManager.OnLevelLoaded += DeleteThrownProjectiles;
        }

        private void OnDisable()
        {
            projectileLevel.OnValueChanged -= OnProjectileLevelChanged;

            GlobalEventManager.OnProjectileThrown -= ProjectileThrown;
            GlobalEventManager.OnLevelLoaded -= DeleteThrownProjectiles;
        }
        public override void SetInteractive(bool interactive)
        {
            _interactive = interactive;
            RespawnProjectile();
        }

        private void OnProjectileLevelChanged()
        {
            RespawnProjectile();

            Debug.Log($"Projectile level: -> {projectileLevel.Value}");
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
            var projectile = projectilePrefabs.Find(p => p.name == projectileName);

            projectile.level = projectileLevel.Value;
            projectile.GetComponent<Follower>().followPoint = spawnPoint;
            _spawnedProjectile = Instantiate(projectile, spawnPoint, Quaternion.identity);
            _spawnedProjectile.SetInteractive(_interactive);
        }

        private void DeleteThrownProjectiles(Level dummy)
        {
            foreach (var projectile in _thrownProjectiles.Where(projectile => projectile != null))
                Destroy(projectile.gameObject);

            _thrownProjectiles.Clear();
        }

        public void SpawnRock()
        {
            SpawnProjectile("Rock");
        }
    }
}