using UnityEngine;
using UnityEngine.U2D;

namespace Tools
{
    [RequireComponent(typeof(SpriteShapeController))]
    public class StringVisualizer : MonoBehaviour
    {
        public GameObject firstCorner;
        public GameObject secondCorner;
        
        private SpriteShapeController _spriteShapeController;

        private void Awake()
        {
            _spriteShapeController = GetComponent<SpriteShapeController>();
            transform.localPosition = Vector2.zero;
        }

        public void Update()
        {
            _spriteShapeController.spline.SetPosition(0, transform.InverseTransformPoint(firstCorner.transform.position));
            _spriteShapeController.spline.SetPosition(1, transform.InverseTransformPoint(secondCorner.transform.position));
        }
    }
}
