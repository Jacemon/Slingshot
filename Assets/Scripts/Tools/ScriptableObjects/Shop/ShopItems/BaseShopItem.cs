using Tools.ScriptableObjects.References;
using UnityEngine;

namespace Tools.ScriptableObjects.Shop.ShopItems
{
    public abstract class BaseShopItem : ScriptableObject
    {
        public Sprite itemIcon;
        public string itemNameKey;
        public BoolReference isPurchased;
        
        public abstract int ItemCost { get; }

        public abstract void Purchase();
    }
}