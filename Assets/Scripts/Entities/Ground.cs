using Entities.Targets;
using Managers;
using UnityEngine;

namespace Entities
{
    public class Ground : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.transform.parent = transform;
            collision.collider.enabled = false;
            collision.rigidbody.isKinematic = true;
            collision.rigidbody.velocity = Vector2.zero;
            collision.rigidbody.angularVelocity = 0;

            if (collision.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.sortingLayerName = "Background";
                spriteRenderer.sortingOrder = 3;
            }

            if (collision.gameObject.TryGetComponent(out Target target))
            {
                GlobalEventManager.OnTargetHitGround?.Invoke(target);
                target.LateDestroy();
            }
        }
    }
}