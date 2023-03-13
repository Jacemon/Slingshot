using Tools.ScriptableObjects;
using UnityEngine;

namespace Managers.Shop.ShopItems
{
    public abstract class BaseShopItem : ScriptableObject
    {
        public Sprite itemIcon;
        public string itemName;
        public IntReference itemLevel;

        public abstract int ItemCost { get; }
    }
}
