using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pouch : MonoBehaviour
{
    [Header("Settings")]
    public float throwSpeed = 10.0f;
    public Vector2 startPosition = new(0, -2);

    [Header("Current parameters")] 
    public bool pouchFill = false;

    private SpringJoint2D[] _springJoints2D;

    private void Awake()
    {
        _springJoints2D = GetComponents<SpringJoint2D>();
    }

    private void Update()
    {
        if (!pouchFill) 
        {
            _springJoints2D[0].enabled = true;
            _springJoints2D[1].enabled = true;
        }
    }
}
