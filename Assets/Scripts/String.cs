using UnityEngine;
using UnityEngine.U2D;

public class String : MonoBehaviour
{
    public GameObject firstCorner;
    public GameObject secondCorner;
    
    private SpriteShapeController _spriteShapeController;

    private void Awake()
    {
        _spriteShapeController = GetComponent<SpriteShapeController>();
        transform.position = Vector2.zero;
    }

    public void Update()
    {
        _spriteShapeController.spline.SetPosition(0, firstCorner.transform.position);
        _spriteShapeController.spline.SetPosition(1, secondCorner.transform.position);
    }
}
