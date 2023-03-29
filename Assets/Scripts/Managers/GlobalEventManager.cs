using System;
using Entities;
using Entities.Targets;

namespace Managers
{
    public static class GlobalEventManager
    {
        // Target
        public static Action<Target> OnTargetHitCart;
        public static Action<Target> OnTargetHitGround;
    
        // Projectile
        public static Action<Projectile> OnProjectileThrow;

        // Level
        public static Action OnLevelLoad;
    }
}