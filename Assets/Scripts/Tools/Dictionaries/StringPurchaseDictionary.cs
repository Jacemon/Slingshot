using Managers;
using RotaryHeart.Lib.SerializableDictionary;

namespace Tools.Dictionaries
{
    [System.Serializable]
    public class StringPurchaseDictionary : SerializableDictionaryBase<string, ShopManager.Purchase> { }
}