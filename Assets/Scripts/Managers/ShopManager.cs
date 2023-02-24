using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ShopManager : MonoBehaviour
    {
        public MoneyManager moneyManager;
        public ProjectileManager projectileManager;

        public Dictionary<int, Purchase> purchases = new();

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
            ReloadPurchases();
        }

        public void ReloadPurchases()
        {
            // todo make cost calculation function for projectiles 
            purchases[0] = new Purchase(projectileManager.projectileLevel * 2,
                () => {
                    projectileManager.projectileLevel++;
                    GlobalEventManager.OnProjectileLevelUpped?.Invoke();
                });
            // сделать так, что если предмет уже куплен, то его цена просто становится 0 и по сути он просто покупает
            // её заново но бесплатно
            purchases[10] = new Purchase(1000, () => Debug.Log("You buy super duper mega skin!"));
        }
        
        public void Buy(int id)
        {
            if (purchases.TryGetValue(id, out var purchase) && moneyManager.WithdrawMoney(purchase.cost))
            {
                purchase.Sell();
                Debug.Log($"{id} was purchased");
                purchases.Remove(id);
            }
            else
            {
                Debug.Log($"{id} was not purchased");
            }
            ReloadPurchases();
        }
    }
}