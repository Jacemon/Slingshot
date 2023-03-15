using System.Collections.Generic;
using System.Linq;
using Tools.Interfaces;
using Tools.ScriptableObjects.Slingshot.SlingshotSkins;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;

namespace Tools.ScriptableObjects.Slingshot
{
    public class SlingshotDisplay : MonoBehaviour, IReloadable
    {
        [FormerlySerializedAs("baseSlingshot")] public BaseSlingshotSkin baseSlingshotSkin;
        [Space] 
        public SpriteRenderer slingshotSpriteRenderer;
        public SpriteRenderer pouchSpriteRenderer;
        public List<SpriteShapeController> stringSpriteShapeControllers;

        private void Awake() // TODO: skin manager
        {
            ReloadData();
        }

        public void ReloadData()
        {
            if (slingshotSpriteRenderer != null) slingshotSpriteRenderer.sprite = baseSlingshotSkin.slingshotSprite;
            if (pouchSpriteRenderer != null) pouchSpriteRenderer.sprite = baseSlingshotSkin.pouchSprite;
            foreach (var spriteShapeController in stringSpriteShapeControllers
                         .Where(stringSpriteRenderer => stringSpriteRenderer != null))
            {
                spriteShapeController.spriteShape = baseSlingshotSkin.stringSpriteShape;
            }
        }
    }
}