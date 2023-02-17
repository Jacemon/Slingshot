using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var dropped = collision.gameObject;
        
        dropped.transform.parent = transform;

        dropped.GetComponent<Rigidbody2D>().isKinematic = true;
        dropped.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        dropped.GetComponent<Rigidbody2D>().angularVelocity = 0;
        dropped.GetComponent<Collider2D>().enabled = false;
        
        var droppedSpriteRenderer = dropped.GetComponentInChildren<SpriteRenderer>();
        if (droppedSpriteRenderer != null)
        {
            droppedSpriteRenderer.sortingLayerName = "Background";
            droppedSpriteRenderer.sortingOrder = 3;
        }
    }
}
