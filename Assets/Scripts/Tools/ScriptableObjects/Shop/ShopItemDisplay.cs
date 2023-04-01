using Lean.Localization;
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
        [Space] 
        public Image iconImage;
        public TextMeshProUGUI nameLabel;
        public TextMeshProUGUI costLabel;
        [Space]
        public TextMeshProUGUI levelLabel;
        public TextMeshProUGUI nextLevelLabel;
        [Space]
        public TextMeshProUGUI arrowLabel;
        public TextMeshProUGUI levelText;
        [Space] 
        public Button buyButton;

        public void Reload()
        {
            if (iconImage != null) iconImage.sprite = shopItem.itemIcon;
            if (nameLabel != null) nameLabel.text = LeanLocalization.GetTranslationText(shopItem.itemNameKey);
            if (costLabel != null)
            {
                if (shopItem.isPurchased != null && shopItem.isPurchased.Value)
                {
                    costLabel.text = LeanLocalization.GetTranslationText("Levels/Shop/PurchaseText");
                }
                else
                {
                    costLabel.text = shopItem.ItemCost != 0 ? 
                        MoneyManager.FormatInteger(shopItem.ItemCost) : 
                        LeanLocalization.GetTranslationText("Levels/Shop/FreeText");
                }
            }
            if (levelLabel != null) levelLabel.text = "";
            if (nextLevelLabel != null) nextLevelLabel.text = "";
            if (arrowLabel != null) arrowLabel.enabled = false;
            if (levelText != null) levelText.enabled = false;
            
            if (shopItem is not LeveledShopItem leveledShopItem) return;
            if (levelLabel != null) levelLabel.text = $"{leveledShopItem.itemLevel.Value}";
            if (nextLevelLabel != null) nextLevelLabel.text = $"{leveledShopItem.itemLevel.Value + 1}";
            if (arrowLabel != null) arrowLabel.enabled = true;
            if (levelText != null) levelText.enabled = true;
        }
    }
}