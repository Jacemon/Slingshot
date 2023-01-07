using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Pouch : MonoBehaviour
{
    [Header("Settigs")]
    public float ThrowSpeed = 250.0f;

    [Header("Current parameters")] 
    public bool PouchFill = false;

    private SpringJoint2D[] SpringJoints2D;

    private void Awake()
    {
        SpringJoints2D = GetComponents<SpringJoint2D>();
    }

    private void Update()
    {
        if (PouchFill)
        {
            /*SpringJoints2D[0].enabled = false;
            SpringJoints2D[1].enabled = false;*/
        }
        else
        {
            SpringJoints2D[0].enabled = true;
            SpringJoints2D[1].enabled = true;
        }
    }
}
