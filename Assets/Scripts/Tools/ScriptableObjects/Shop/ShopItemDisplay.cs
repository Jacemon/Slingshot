using Managers;
using TMPro;
using Tools.Interfaces;
using Tools.ScriptableObjects.Shop.ShopItems;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.ScriptableObjects.Shop
{
    public class ShopItemDisplay : MonoBehaviour, IReloadable
    {
        public BaseShopItem shopItem;

        [Space] public Image iconImage;

        public TextMeshProUGUI nameLabel;
        public TextMeshProUGUI costLabel;
        public TextMeshProUGUI levelLabel;

        [Space] public Button buyButton;

        public void ReloadData()
        {
            if (iconImage != null && shopItem.itemIcon != null) iconImage.sprite = shopItem.itemIcon;
            if (nameLabel != null && shopItem.itemName != null) nameLabel.text = shopItem.itemName;
            if (costLabel != null)
                costLabel.text = shopItem.ItemCost != 0 ? MoneyManager.FormatInteger(shopItem.ItemCost) : "Free";
            if (levelLabel != null)
                levelLabel.text = shopItem is LeveledShopItem leveledShopItem
                    ? $"{leveledShopItem.itemLevel.Value}->{leveledShopItem.itemLevel.Value + 1}lvl"
                    : "";
        }
    }
}