using UnityEngine;

public class Target : MonoBehaviour
{
    public int MaxHealth = 1;
    public int Health;

    private void Awake()
    {
        Health = MaxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || !collision.gameObject.CompareTag("Throwed projectile"))
        {
            return;
        }
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        projectile.transform.parent = transform;
        projectile.GetComponent<Rigidbody2D>().isKinematic = true;
        projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        projectile.GetComponent<Collider2D>().enabled = false;
    }
}
