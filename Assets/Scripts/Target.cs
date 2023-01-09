using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Destroyable))]
public class Target : MonoBehaviour
{
    [Header("Settings")] 
    public string targetName = "None";
    [Space]
    public int maxHealth = 3;
    public int points = 100;
    [Space]
    public Slider slider;
    
    [SerializeField]
    private int health;

    private void Awake()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || !collision.gameObject.CompareTag("Projectile"))
        {
            return;
        }

        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
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
            gameObject.layer = LayerMask.NameToLayer("Background");
        }

        slider.value = health;
    }
}
