using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Pouch : MonoBehaviour
{
    public bool PouchFill = false;
    public float ThrowSpeed = 250.0f;

    private SpringJoint2D[] springJoints2D;

    private void Awake()
    {
        springJoints2D = GetComponents<SpringJoint2D>();
    }

    private void Update()
    {
        if (PouchFill)
        {
            /*springJoints2D[0].enabled = false;
            springJoints2D[1].enabled = false;*/
        }
        else
        {
            springJoints2D[0].enabled = true;
            springJoints2D[1].enabled = true;
        }
    }
}
