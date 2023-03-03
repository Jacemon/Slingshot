using System;
using Tools;
using Tools.Dictionaries;
using Tools.Interfaces;
using UnityEngine;

namespace Managers
{
    public class ShopManager : MonoBehaviour, IReloadable
    {
        public MoneyManager moneyManager;
        public ProjectileManager projectileManager;
        public LevelManager levelManager;
        [Space]
        public IntLinearCurve projectileCostCurve;
        public IntLinearCurve levelCostCurve;
        [Space]
        public StringPurchaseDictionary purchases;
        public StringTextMeshProUGUIDictionary purchaseLabels;

        [Serializable]
        public class Purchase
        {
            public int cost;
            public Action action;

            public Purchase(int cost, Action action)
            {
                this.cost = cost;
                this.action = action;
            }

            public void Sell()
            {
                action?.Invoke();
            }
        }

        public void Awake()
        {
            GlobalEventManager.OnLoad.AddListener(ReloadData);
            ReloadData();
        }

        public void Buy(string key)
        {
            if (purchases.TryGetValue(key, out var purchase) && moneyManager.WithdrawMoney(purchase.cost))
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
            purchases["projectileLevel"] = new Purchase(
                projectileCostCurve.ForceEvaluate(projectileManager.projectileLevel),
                () =>
                {
                    GlobalEventManager.OnProjectileLevelUp?.Invoke();
                }
            );
            purchases["maxAvailableLevel"] = new Purchase(
                levelCostCurve.ForceEvaluate(levelManager.maxAvailableLevel),
                () =>
                {
                    GlobalEventManager.OnLevelUp?.Invoke();
                }
            );
            // сделать так, что если предмет уже куплен, то его цена просто становится 0, и по сути он просто покупает
            // его заново, но бесплатно
            purchases["superSkin"] = new Purchase(1000, () => Debug.Log("You buy super duper mega skin!"));

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