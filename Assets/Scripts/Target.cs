using UnityEngine;

[RequireComponent(typeof(Destroyable))]
public class Target : MonoBehaviour
{
    [Header("Settings")] 
    public string targetName = "None";
    [Space]
    public int maxHealth = 3;
    public int points = 100;
    
    [SerializeField]
    private int health;

    private void Awake()
    {
        health = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || !collision.gameObject.CompareTag("Thrown Projectile"))
        {
            return;
        }

        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        //EnterProjectile(projectile);
        GetDamage(projectile.damage);
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{targetName} get {damage} damage ({health}/{maxHealth})");
        if (health <= 0)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            Debug.Log($"{targetName} shot down");
        }
    }
    
    private void EnterProjectile(Projectile projectile)
    {
        projectile.transform.parent = transform;
        projectile.GetComponent<Rigidbody2D>().isKinematic = true;
        projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        projectile.GetComponent<Collider2D>().enabled = false;
    }

    private void ExitProjectile(Projectile projectile)
    {
        projectile.transform.parent = null;
        projectile.GetComponent<Rigidbody2D>().isKinematic = false;
        projectile.GetComponent<Collider2D>().enabled = true;
    }
}
