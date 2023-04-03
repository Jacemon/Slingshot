using System;
using UnityEngine;

namespace Tools.ScriptableObjects.Shop.ShopItems
{
    [CreateAssetMenu(fileName = "StaticCostShopItem", menuName = "Custom/Shop Item/Static Cost Shop Item")]
    public class StaticCostShopItem : FreeShopItem
    {
        public int cost;

        public override int ItemCost => cost;
    }
}