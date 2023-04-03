using Tools.ScriptableObjects.References;

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