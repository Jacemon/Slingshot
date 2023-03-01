using Entities;
using UnityEngine.Events;

namespace Managers
{
    public static class GlobalEventManager
    {
        // Target
        public static readonly UnityEvent<Target> OnTargetGetDamage = new();
        public static readonly UnityEvent<Target> OnTargetHitCart = new();
        public static readonly UnityEvent<Target> OnTargetHitGround = new();
    
        // Projectile
        public static readonly UnityEvent<Projectile> OnProjectileThrown = new();

        // Saving
        public static readonly UnityEvent OnSave = new();
        public static readonly UnityEvent OnLoad = new();
        
        // Level
        public static readonly UnityEvent OnLevelSwitched = new();
        
        // Upgrades
        public static readonly UnityEvent OnProjectileLevelUpped = new();
        public static readonly UnityEvent OnLevelUpped = new();
    }
}
