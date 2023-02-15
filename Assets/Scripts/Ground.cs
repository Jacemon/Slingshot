using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject dropped = collision.gameObject;
        
        dropped.transform.parent = transform;
        Vector2 projectileLocalPosition = dropped.transform.localPosition;
        dropped.transform.localPosition = projectileLocalPosition;
        
        dropped.GetComponent<Rigidbody2D>().isKinematic = true;
        dropped.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        dropped.GetComponent<Rigidbody2D>().angularVelocity = 0;
        dropped.GetComponent<Collider2D>().enabled = false;
        
        var droppedSpriteRenderer = dropped.GetComponentInChildren<SpriteRenderer>();
        if (droppedSpriteRenderer != null)
        {
            droppedSpriteRenderer.sortingLayerName = "Back";
            droppedSpriteRenderer.sortingOrder = 3;
        }
    }
}
