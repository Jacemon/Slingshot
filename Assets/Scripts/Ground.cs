using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnterGameObject(collision.gameObject);
    }

    private void EnterGameObject(GameObject projectile)
    {
        projectile.transform.parent = transform;
        projectile.GetComponent<Rigidbody2D>().isKinematic = true;
        projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        projectile.GetComponent<Rigidbody2D>().angularVelocity = 0;
        projectile.GetComponent<Collider2D>().enabled = false;
    }
}
