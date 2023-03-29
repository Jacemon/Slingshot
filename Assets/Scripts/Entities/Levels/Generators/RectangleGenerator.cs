using System;
using Managers;
using UnityEditor;
using UnityEngine;

namespace Entities.Levels.Generators
{
    [Serializable]
    public class RectangleGenerator : BaseGenerator
    {
        [Header("Rectangle settings")] 
        public Rect rectangle;
        [Space]
        public int targetsAmount;
        public float spaceBetween;
        
        protected override void StartGenerate()
        {
            var scale = parent != null ? parent.localScale : new Vector3(1, 1, 1);
            var localRectangle = parent != null ? new Rect(
                parent.TransformPoint(rectangle.x, rectangle.y, 0),
                new Vector2(rectangle.width * scale.x, rectangle.height * scale.y)
            ) : rectangle;
            generatedTargets = TargetManager.GenerateTargetsByRectangle(
                localRectangle,
                targetsAmount,
                spaceBetween * scale.x,
                randomTargets, 
                level, 
                parent,
                minMaxScale
            );
        }

        public override void DrawGizmos()
        {
            var scale = parent != null ? parent.localScale : new Vector3(1, 1, 1);
            var localRectangle = new Rect(
                parent != null ? 
                    parent.TransformPoint(rectangle.x, rectangle.y, 0) : 
                    new Vector2(rectangle.x, rectangle.y),
                new Vector2(rectangle.width * scale.x, rectangle.height * scale.y)
            );
            
            Handles.DrawSolidRectangleWithOutline(localRectangle, Color.clear, Color.green);
        }
    }
}