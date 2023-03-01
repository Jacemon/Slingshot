using System;
using Tools;
using Tools.Dictionaries;
using UnityEngine;

namespace Managers
{
    public class ShopManager : MonoBehaviour, IReloadable
    {
        public MoneyManager moneyManager;
        public ProjectileManager projectileManager;
        public LevelManager levelManager;
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
            GlobalEventManager.OnLoad.AddListener(Reload);
            Reload();
        }

        public void Buy(string key)
        {
            if (purchases.TryGetValue(key, out var purchase) && moneyManager.WithdrawMoney(purchase.cost))
            {
                purchase.Sell();
                Debug.Log($"{key} was purchased");
                purchases.Remove(key);
            }
            else
            {
                Debug.Log($"{key} was not purchased");
            }
            Reload();
        }
        
        public void Reload()
        {
            // todo make cost calculation functions
            purchases["projectileLevel"] = new Purchase(projectileManager.projectileLevel * 2,
                () => 
                {
                    projectileManager.projectileLevel++;
                    GlobalEventManager.OnProjectileLevelUpped?.Invoke();
                });
            purchases["maxAvailableLevel"] = new Purchase(1 + levelManager.maxAvailableLevel * 40,
                () =>
                {
                    levelManager.maxAvailableLevel++;
                    GlobalEventManager.OnLevelUpped?.Invoke();
                });
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