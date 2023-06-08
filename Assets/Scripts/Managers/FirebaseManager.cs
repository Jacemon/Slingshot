using Entities;
using Entities.Levels;
using Entities.Targets;
using Firebase;
using Firebase.Analytics;
using Tools.ScriptableObjects.References;
using Tools.ScriptableObjects.Shop.ShopItems;
using UnityEngine;

namespace Managers
{
    public class FirebaseManager : MonoBehaviour
    {
        public BoolReference firstPlay;
        
        private void Awake()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(_ =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        }

        private void OnEnable()
        {
            GlobalEventManager.OnLevelLoaded += FirstPlay; // TODO: mb remake without level loading
            GlobalEventManager.OnTutorialComplete += TutorialComplete;
            GlobalEventManager.OnInterstitialAdImpression += InterstitialAdImpression;
            GlobalEventManager.OnRewardedAdImpression += RewardedAdImpression;
            //GlobalEventManager.OnItemPurchased += ItemPurchased;
            GlobalEventManager.OnItemPurchased += ItemPurchasedWithoutParameters;
            GlobalEventManager.OnProjectileThrown += ProjectileThrown;
            GlobalEventManager.OnProjectileHitTarget += ProjectileHitTarget;
            GlobalEventManager.OnTargetHitGround += TargetHitGround;
            GlobalEventManager.OnTargetHitCart += TargetHitCart;
        }

        private void OnDisable()
        {
            GlobalEventManager.OnLevelLoaded -= FirstPlay;
            GlobalEventManager.OnTutorialComplete -= TutorialComplete;
            GlobalEventManager.OnInterstitialAdImpression -= InterstitialAdImpression;
            GlobalEventManager.OnRewardedAdImpression -= RewardedAdImpression;
            //GlobalEventManager.OnItemPurchased -= ItemPurchased;
            GlobalEventManager.OnItemPurchased -= ItemPurchasedWithoutParameters;
            GlobalEventManager.OnProjectileThrown -= ProjectileThrown;
            GlobalEventManager.OnProjectileHitTarget -= ProjectileHitTarget;
            GlobalEventManager.OnTargetHitGround -= TargetHitGround;
            GlobalEventManager.OnTargetHitCart -= TargetHitCart;
        }

        #region FirebaseEventHandlers

        private void FirstPlay(Level _)
        {
            if (!firstPlay.Value)
            {
                firstPlay.Value = true;
                FirebaseAnalytics.LogEvent("first_play");
            }
        }

        private void TutorialComplete()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventTutorialComplete);
        }
        
        private void InterstitialAdImpression()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression,
                new Parameter("ad_type", "interstitial"));
        }
        
        private void RewardedAdImpression()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression,
                new Parameter("ad_type", "rewarded"));
        }

        private void ItemPurchased(BaseShopItem shopItem)
        {
            switch (shopItem)
            {
                case LeveledShopItem leveledShopItem:
                    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase, new Parameter[] 
                        {
                            new ("item_type", "leveled_item"),
                            new ("item_name", leveledShopItem.name),
                            new (FirebaseAnalytics.ParameterLevel, leveledShopItem.itemLevel.Value)
                        }
                    );
                    break;
                default:
                    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase, new Parameter[]
                        {
                            new("item_type", "base_item"),
                            new("item_name", shopItem.name)
                        }
                    );
                    break;
            }
        }

        private void ItemPurchasedWithoutParameters(BaseShopItem shopItem) // TODO: delete -> ItemPurchased()
        {
            switch (shopItem)
            {
                case LeveledShopItem leveledShopItem:
                    FirebaseAnalytics.LogEvent($"{FirebaseAnalytics.EventPurchase}_{leveledShopItem.name}" +
                                               $"_{leveledShopItem.itemLevel.Value}, ");
                    break;
                default:
                    FirebaseAnalytics.LogEvent($"{FirebaseAnalytics.EventPurchase}_{shopItem.name}");
                    break;
            }
        }

        // private void LevelUp(Level level)
        // {
        //     FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelUp, new Parameter("level", level.levelNumber));
        // }
        //
        // private void ProjectileUpgrade(Projectile projectile)
        // {
        //     FirebaseAnalytics.LogEvent("projectile_upgrade", new Parameter[] 
        //         {
        //             new ("projectile_type", projectile.projectileName),
        //             new ("level", projectile.level)
        //         }
        //     );
        // }
        
        private void ProjectileThrown(Projectile projectile)
        {
            FirebaseAnalytics.LogEvent("projectile_thrown", 
                new Parameter("projectile_type", projectile.projectileName));
        }

        private void ProjectileHitTarget(Projectile projectile, Target target)
        {
            FirebaseAnalytics.LogEvent("projectile_hit_target", new Parameter[] 
                {
                    new ("projectile_type", projectile.projectileName),
                    new ("target_type", target.targetName)
                }
            );
        }

        private void TargetHitGround(Target target)
        {
            FirebaseAnalytics.LogEvent("target_hit_ground", new Parameter("target_type", target.targetName));
        }
        
        private void TargetHitCart(Target target)
        {
            FirebaseAnalytics.LogEvent("target_hit_cart", new Parameter("target_type", target.targetName));
        }
        
        #endregion
    }
}
