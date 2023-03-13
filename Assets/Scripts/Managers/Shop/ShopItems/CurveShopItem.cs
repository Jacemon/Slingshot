using Tools;
using UnityEngine;

namespace Managers.Shop.ShopItems
{
    [CreateAssetMenu(fileName = "CurveShopItem", menuName = "Custom/Shop Item/Curve Shop Item")]
    public class CurveShopItem : FreeShopItem
    {
        public LinearCurve itemCostCurve = new();

        public override int ItemCost => itemCostCurve.Evaluate(itemLevel.value);
    }
}
