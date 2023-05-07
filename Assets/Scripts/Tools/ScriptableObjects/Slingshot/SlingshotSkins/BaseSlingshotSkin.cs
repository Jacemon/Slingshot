using System;
using UnityEngine;
using UnityEngine.U2D;

namespace Tools.ScriptableObjects.Slingshot.SlingshotSkins
{
    [CreateAssetMenu(fileName = "BaseSlingshotSkin", menuName = "Custom/Slingshot/Base Slingshot Skin")]
    public class BaseSlingshotSkin : ScriptableObject, IEquatable<BaseSlingshotSkin>
    {
        public string slingshotName;
        public Sprite slingshotSprite;
        public Sprite pouchSprite;
        public SpriteShape stringSpriteShape;

        public bool Equals(BaseSlingshotSkin other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return slingshotName == other.slingshotName;
        }

        public override int GetHashCode()
        {
            return slingshotName == null ? base.GetHashCode() : slingshotName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BaseSlingshotSkin)obj);
        }
    }
}