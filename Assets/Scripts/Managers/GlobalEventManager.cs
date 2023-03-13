using System;
using Entities;

namespace Managers
{
    public static class GlobalEventManager
    {
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
    }
}
