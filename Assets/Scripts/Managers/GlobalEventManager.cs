using Entities;
using UnityEngine.Events;

namespace Managers
{
    public static class GlobalEventManager
    {
        public static readonly UnityEvent<Target> OnTargetSpawned = new(); // useless yet
        public static readonly UnityEvent<Target> OnTargetGetDamage = new();
        public static readonly UnityEvent<Target> OnTargetHitCart = new();
    
        public static readonly UnityEvent<Projectile> OnProjectileSpawned = new(); // useless yet
        public static readonly UnityEvent<Projectile> OnProjectileThrown = new();

        public static readonly UnityEvent OnLevelSwitched = new();
    }
}
