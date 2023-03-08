using System;
using Tools;
using Tools.Dictionaries;
using Tools.Interfaces;
using UnityEngine;

namespace Managers
{
    public class ShopManager : MonoBehaviour, IReloadable
    {
        public int projectileLevel;
        public int maxAvailableLevel;
        [Space]
        public IntLinearCurve projectileCostCurve;
        public IntLinearCurve levelCostCurve;
        [Space]
        public StringPurchaseDictionary purchases;
        public StringTextMeshProUGUIDictionary purchaseLabels;

        [Serializable]
        public class Purchase
        {
            public long cost;
            public Action action;

            public Purchase(long cost, Action action)
            {
                this.cost = cost;
                this.action = action;
            }

            public void Sell()
            {
                action?.Invoke();
            }
        }

        private void Awake()
        {
            GlobalEventManager.UnityEvents.OnLoad.AddListener(ReloadData);
        }

        public void Buy(string key)
        {
            if (purchases.TryGetValue(key, out var purchase) && 
                GlobalEventManager.onMoneyWithdraw.Invoke(purchase.cost))
            {
                purchase.Sell();
                purchases.Remove(key);
                
                Debug.Log($"{key} was purchased");
            }
            else
            {
                Debug.Log($"{key} was not purchased");
            }
            ReloadData();
        }

        public void ReloadData()
        {
            // Purchases reloading
            projectileLevel = GlobalEventManager.onProjectileLevelUp.Invoke(0);
            maxAvailableLevel = GlobalEventManager.onLevelUp.Invoke(0);
            
            purchases["projectileLevel"] = new Purchase(
                projectileCostCurve.ForceEvaluate(projectileLevel),
                () =>
                {
                    GlobalEventManager.onProjectileLevelUp.Invoke(1);
                }
            );
            purchases["maxAvailableLevel"] = new Purchase(
                levelCostCurve.ForceEvaluate(maxAvailableLevel),
                () =>
                {
                    GlobalEventManager.onLevelUp.Invoke(1);
                }
            );
            // сделать так, что если предмет уже куплен, то его цена просто становится 0, и по сути он просто покупает
            // его заново, но бесплатно
            purchases["superSkin"] = new Purchase(1000, () => Debug.Log("You buy super duper mega skin!"));
            
            // Labels reloading
            foreach (var purchase in purchases)
            {
                if (purchaseLabels.TryGetValue(purchase.Key, out var label) && label != null)
                {
                    label.SetText(purchase.Value.cost.ToString());
                }
            }
            
            Debug.Log("Shop was reloaded");
        }
    }
}