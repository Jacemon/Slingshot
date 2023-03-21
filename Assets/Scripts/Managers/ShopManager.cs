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
        public GameObject shopPointPrefab;
        public VerticalLayoutGroup verticalLayoutGroup;
        [Space]
        [Tooltip("Shop Item Display can be null, instead it will be displayed in the Vertical Layout Group")]
        public List<ShopItemWithDisplay> shopItemWithDisplays;
        [Space] 
        public Animator shopAnimator;
        public float shopAnimationSpeed = 1f;
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        [Serializable]
        public class ShopItemWithDisplay
        {
            public BaseShopItem shopItem;
            public ShopItemDisplay shopItemDisplay;
        }

        private void Awake()
        {
            ReloadData();
        }

        public void ReloadData()
        {
            foreach (var shopItemWithDisplay in shopItemWithDisplays)
            {
                if (shopItemWithDisplay.shopItemDisplay == null)
                {
                    shopItemWithDisplay.shopItemDisplay = Instantiate(shopPointPrefab, verticalLayoutGroup.transform)
                        .GetComponent<ShopItemDisplay>();
                }

                shopItemWithDisplay.shopItemDisplay.shopItem = shopItemWithDisplay.shopItem;
                shopItemWithDisplay.shopItemDisplay.buyButton.onClick.RemoveAllListeners();
                shopItemWithDisplay.shopItemDisplay.buyButton.onClick.AddListener(
                    () => Buy(shopItemWithDisplay.shopItem.itemName));
                shopItemWithDisplay.shopItemDisplay.ReloadData();
            }
        }

        private void Buy(string key)
        {
            var shopItem = shopItemWithDisplays.Find(item => item.shopItem.itemName == key).shopItem;
            if (shopItem != null &&
                ((shopItem.isPurchased != null && shopItem.isPurchased.Value) || 
                MoneyManager.WithdrawMoney(shopItem.ItemCost))
                )
            {
                    shopItem.Purchase();
                    Debug.Log($"{key} was purchased");
            }
            else
            {
                Debug.Log($"{key} was not purchased");
            }
            ReloadData();
        }

        public void ToggleShop(bool isOpen)
        {
            shopAnimator.speed = shopAnimationSpeed;
            shopAnimator.SetBool(IsOpen, isOpen);
        }
    }
}
