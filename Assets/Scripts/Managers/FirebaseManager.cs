using Entities;
using Entities.Targets;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace Managers
{
    public class FirebaseManager : MonoBehaviour
    {
        private void Awake()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(_ =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        }

        private void OnEnable()
        {
            GlobalEventManager.OnProjectileHitTarget += ProjectileHitTarget;
        }

        private void OnDisable()
        {
            GlobalEventManager.OnProjectileHitTarget += ProjectileHitTarget;
        }

        private void ProjectileHitTarget(Projectile projectile, Target target)
        {
            FirebaseAnalytics.LogEvent("projectile_hit_target", new Parameter[] {
                    new ("projectile_type", projectile.projectileName),
                    new ("target_type", target.targetName)
                }
            );
        }
    }
}
