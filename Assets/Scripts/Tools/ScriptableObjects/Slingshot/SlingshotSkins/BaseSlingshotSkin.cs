using UnityEngine;
using UnityEngine.U2D;

namespace Tools.ScriptableObjects.Slingshot.SlingshotSkins
{
    [CreateAssetMenu(fileName = "BaseSlingshotSkin", menuName = "Custom/Slingshot/Base Slingshot Skin")]
    public class BaseSlingshotSkin : ScriptableObject
    {
        public string slingshotName;
        public Sprite slingshotSprite;
        public Sprite pouchSprite;
        public SpriteShape stringSpriteShape;
    }
}