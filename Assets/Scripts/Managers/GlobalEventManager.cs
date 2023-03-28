using System;
using Entities;
using Entities.Targets;

namespace Managers
{
    public static class GlobalEventManager
    {
        // Target
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