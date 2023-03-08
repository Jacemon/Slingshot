using System;
using Entities;
using UnityEngine.Events;

namespace Managers
{
    public static class GlobalEventManager
    {
        public static class UnityEvents
        {
            // Saving
            public static readonly UnityEvent OnSave = new();
            public static readonly UnityEvent OnLoad = new();
        }
        
        // Target
        public static Action<Target> onTargetGetDamage;
        public static Action<Target> onTargetHitCart;
        public static Action<Target> onTargetHitGround;
    
        // Projectile
        public static Action<Projectile> onProjectileThrow;

        // Money
        public static Predicate<long> onMoneyWithdraw = _ => false;
        
        // Level
        public static Action onLevelLoad;
        
        // Upgrades
        public static Func<int, int> onProjectileLevelUp = _ => 0;
        public static Func<int, int> onLevelUp = _ => 0;
    }
}
