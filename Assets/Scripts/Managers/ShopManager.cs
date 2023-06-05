using System;
using System.Collections.Generic;
using Tools.Interfaces;
using Tools.ScriptableObjects.Shop;
using Tools.ScriptableObjects.Shop.ShopItems;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ShopManager : MonoBehaviour, IReloadable
    {
        public ShopItemDisplay shopItemPrefab;
        public VerticalLayoutGroup verticalLayoutGroup;
        [Space]
        [Tooltip("Shop Item Display can be null, instead it will be displayed in the Vertical Layout Group")]
        public List<ShopItemWithDisplay> shopItemWithDisplays;
        [Space]
        public Animator shopAnimator;
        public float shopAnimationSpeed = 1f;
        
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void Awake()
        {
            Reload();
        }

        public void Reload()
        {
            foreach (var shopItemWithDisplay in shopItemWithDisplays)
            {
                if (shopItemWithDisplay.shopItemDisplay == null)
                    shopItemWithDisplay.shopItemDisplay = Instantiate(shopItemPrefab, verticalLayoutGroup.transform);

                shopItemWithDisplay.shopItemDisplay.shopItem = shopItemWithDisplay.shopItem;
                shopItemWithDisplay.shopItemDisplay.buyButton.onClick.RemoveAllListeners();
                shopItemWithDisplay.shopItemDisplay.buyButton.onClick.AddListener(
                    () => Buy(shopItemWithDisplay.shopItem.itemNameKey));
                shopItemWithDisplay.shopItemDisplay.Reload();
            }
        }

        private void Buy(string key)
        {
            var shopItem = shopItemWithDisplays.Find(item => item.shopItem.itemNameKey == key).shopItem;
            if (shopItem != null &&
                ((shopItem.isPurchased != null && shopItem.isPurchased.Value) ||
                 MoneyManager.WithdrawMoney(shopItem.ItemCost)))
            {
                shopItem.Purchase();
                if (!shopItem.isPurchased.Value)
                {
                    GlobalEventManager.OnItemPurchased?.Invoke(shopItem);
                    Debug.Log($"{key} was purchased");
                }
                else
                {
                    Debug.Log($"{key} was selected");
                }
            }
            else
            {
                Debug.Log($"{key} was not purchased");
            }

            Reload();
        }

        public void ToggleShop(bool isOpen)
        {
            shopAnimator.speed = shopAnimationSpeed;
            shopAnimator.SetBool(IsOpen, isOpen);
        }

        [Serializable]
        public class ShopItemWithDisplay
        {
            public BaseShopItem shopItem;
            public ShopItemDisplay shopItemDisplay;
        }
    }
}