using Tools.ScriptableObjects.Reference;

namespace Tools.ScriptableObjects.Shop.ShopItems
{
    public abstract class LeveledShopItem : BaseShopItem
    {
        public IntReference itemLevel;

        public override void Purchase()
        {
            itemLevel.Value++;
        }
    }
}