using System;
using UnityEngine;

public class Cart : MonoBehaviour
{
    [Header("Settings")] 
    public string cartName = "None";

    [Space] 
    public Vector2[] positions;
    public float velocity;

    [Header("Current parameters")]
    public bool isStopped;
    [SerializeField] private int positionIndex;

    private const float ErrorRate = 0.1f;

    private void FixedUpdate()
    {
        if (isStopped)
        {
            return;
        }
        
        transform.position = Vector2.MoveTowards(
            transform.position,
            positions[positionIndex],
            velocity
            );
        if (Vector2.Distance(transform.position, positions[positionIndex]) < ErrorRate)
        {
            NextPosition();
        }
    }

    private void NextPosition()
    {
        positionIndex++;
        if (positionIndex == positions.Length)
        {
            positionIndex = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{collision.gameObject.tag} collided with {cartName}");
        collision.gameObject.GetComponent<Destroyable>()?.Destroy();
    }
}
