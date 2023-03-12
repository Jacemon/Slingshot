using System;
using Entities;
using UnityEngine.Events;

namespace Managers
{
    public static class GlobalEventManager
    {
        // Saving
        public static Action onSave;
        public static Action onLoad;
        
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
