using System;
using System.Collections.Generic;
using Managers.Shop.ShopItems;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.Shop
{
    public class Shop : MonoBehaviour
    {
        public GameObject shopPointPrefab;
        public VerticalLayoutGroup verticalLayoutGroup;
        [Space]
        public List<BaseShopItem> shopItems;

        private void Awake()
        {
            Reload();
        }

        public void Reload()
        {
            foreach (var item in shopItems)
            {
                var shopPoint = Instantiate(shopPointPrefab, verticalLayoutGroup.transform)
                    .GetComponent<ShopItemDisplay>();
                shopPoint.shopItem = item;
                shopPoint.buyButton.onClick.AddListener(() => Buy(item.itemName));
                shopPoint.Reload();
            }
        }

        private void Buy(string key)
        {
            var item = shopItems.Find(item => item.itemName == key);
            if (item != null && GlobalEventManager.onMoneyWithdraw.Invoke(item.ItemCost))
            {
                Debug.Log($"{key} was purchased");
            }
            else
            {
                Debug.Log($"{key} was not purchased");
            }
        }
    }
}
