using System;
using Entities;
using Entities.Levels;
using Entities.Targets;
using Tools.ScriptableObjects.Shop.ShopItems;

namespace Managers
{
    public static class GlobalEventManager
    {
        // Target
        public static Action<Target> OnTargetHitCart;
        public static Action<Target> OnTargetHitGround;

        // Projectile
        public static Action<Projectile> OnProjectileThrown;
        public static Action<Projectile, Target> OnProjectileHitTarget;

        // Level
        public static Action OnTutorialComplete; 
        public static Action<Level> OnLevelLoaded;
        
        // Shop item
        public static Action<BaseShopItem> OnItemPurchased;
        
        // Ad
        public static Action OnInterstitialAdImpression;
        public static Action OnRewardedAdImpression;
    }
}