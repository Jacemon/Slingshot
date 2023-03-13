using UnityEngine;

namespace Managers.Shop.ShopItems
{
    [CreateAssetMenu(fileName = "FreeShopItem", menuName = "Custom/Shop Item/Free Shop Item")]
    public class FreeShopItem : BaseShopItem
    {
        public override int ItemCost => 0;
    }
}
