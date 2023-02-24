using Entities;
using UnityEngine.Events;

namespace Managers
{
    public static class GlobalEventManager
    {
        public static readonly UnityEvent<Target> OnTargetSpawned = new();
        public static readonly UnityEvent<Target> OnTargetGetDamage = new();
        public static readonly UnityEvent<Target> OnTargetHitCart = new();
        public static readonly UnityEvent<Target> OnTargetHitGround = new();
    
        public static readonly UnityEvent<Projectile> OnProjectileSpawned = new();
        public static readonly UnityEvent<Projectile> OnProjectileThrown = new();
        public static readonly UnityEvent OnProjectileLevelUpped = new();

        public static readonly UnityEvent OnSave = new();
        public static readonly UnityEvent OnLoad = new();
        
        public static readonly UnityEvent OnLevelSwitched = new();
    }
}
