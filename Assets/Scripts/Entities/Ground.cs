using Managers;
using UnityEngine;

namespace Entities
{
    public class Ground : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var dropped = collision.gameObject;
            var droppedRigidbody = dropped.GetComponent<Rigidbody2D>();
        
            dropped.transform.parent = transform;

            droppedRigidbody.isKinematic = true;
            droppedRigidbody.velocity = Vector2.zero;
            droppedRigidbody.angularVelocity = 0;
            
            dropped.GetComponent<Collider2D>().enabled = false;
        
            var droppedSpriteRenderer = dropped.GetComponentInChildren<SpriteRenderer>();
            if (droppedSpriteRenderer != null)
            {
                droppedSpriteRenderer.sortingLayerName = "Background";
                droppedSpriteRenderer.sortingOrder = 3;
            }
            
            var target = dropped.GetComponent<Target>();
            if (target != null)
            {
                GlobalEventManager.onTargetHitGround?.Invoke(target);
                target.LateDestroy();
            }
        }
    }
}
