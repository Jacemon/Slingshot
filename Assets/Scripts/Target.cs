using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Settigs")]
    public int MaxHealth = 3;
    public int Health;

    private void Awake()
    {
        Health = MaxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || !collision.gameObject.CompareTag("Thrown Projectile"))
        {
            return;
        }

        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        Health-= projectile.Damage;
        if (Health <= 0)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
        }

        projectile.transform.parent = transform;
        projectile.GetComponent<Rigidbody2D>().isKinematic = true;
        projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        projectile.GetComponent<Collider2D>().enabled = false;
    }
}
