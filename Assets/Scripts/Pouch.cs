using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]

public class Pouch : MonoBehaviour
{
    [Header("Settings")]
    public float throwSpeed = 250.0f;
    public Vector2 startPosition = new Vector2(0, -2);

    [Header("Current parameters")] 
    public bool pouchFill = false;

    private SpringJoint2D[] _springJoints2D;

    private void Awake()
    {
        _springJoints2D = GetComponents<SpringJoint2D>();
    }

    private void Update()
    {
        if (pouchFill)
        {
            /*SpringJoints2D[0].enabled = false;
            SpringJoints2D[1].enabled = false;*/
        }
        else
        {
            _springJoints2D[0].enabled = true;
            _springJoints2D[1].enabled = true;
        }
    }
}
