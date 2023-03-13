using Managers.Shop.ShopItems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.Shop
{
    public class ShopItemDisplay : MonoBehaviour
    {
        public BaseShopItem shopItem;
        [Space] 
        public Image iconImage;
        public TextMeshProUGUI nameLabel;
        public TextMeshProUGUI costLabel;
        public TextMeshProUGUI levelLabel;
        [Space]
        public Button buyButton;

        public void Reload()
        {
            iconImage.sprite = shopItem.itemIcon;
            nameLabel.text = shopItem.itemName;
            costLabel.text = MoneyManager.FormatInteger(shopItem.ItemCost);
            levelLabel.text = $"{shopItem.itemLevel}->{shopItem.itemLevel.value + 1}lvl";
        }
    }
}