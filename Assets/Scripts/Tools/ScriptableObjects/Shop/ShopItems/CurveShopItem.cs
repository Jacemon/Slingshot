using UnityEngine;

namespace Tools.ScriptableObjects.Shop.ShopItems
{
    [CreateAssetMenu(fileName = "CurveShopItem", menuName = "Custom/Shop Item/Curve Shop Item")]
    public class CurveShopItem : LeveledShopItem
    {
        public IntLinearCurve itemCostCurve = new();

        public override int ItemCost => itemCostCurve.ForceEvaluate(itemLevel.Value);
    }
}