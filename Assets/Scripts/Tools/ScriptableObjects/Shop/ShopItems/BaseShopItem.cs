using UnityEngine;

namespace Tools.ScriptableObjects.Shop.ShopItems
{
    public abstract class BaseShopItem : ScriptableObject
    {
        public Sprite itemIcon;
        public string itemName;

        public abstract int ItemCost { get; }

        public abstract void Purchase();
    }
}