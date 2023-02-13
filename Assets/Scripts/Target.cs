using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [Header("Settings")] 
    public string targetName = "None";
    [Space]
    public int maxHealth = 3;
    public int points = 100;
    
    [SerializeField]
    private int health;
    
    private Slider _slider;
    private Canvas _healthBar;

    private const float TimeBeforeDestroy = 2f; 
    private const float ShotColliderRadius = 0.25f;

    private void Awake()
    {
        GlobalEventManager.OnTargetSpawned.Invoke(this);
        
        health = maxHealth;

        _healthBar = GetComponentInChildren<Canvas>();
        _slider = _healthBar.GetComponentInChildren<Slider>();

        _healthBar.enabled = false;
        _slider.maxValue = maxHealth;
        _slider.value = health;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || !collision.gameObject.CompareTag("Projectile"))
        {
            return;
        }

        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        // todo
        //projectile.GetComponent<Collider2D>().enabled = false;
        //projectile.GetRandomForce();
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
            _healthBar.enabled = false;
            GetComponent<CircleCollider2D>().radius = ShotColliderRadius;

            Invoke(nameof(LateDestroy), TimeBeforeDestroy);
            
        } 
        else if (health < maxHealth)
        {
            _healthBar.enabled = true;
            _slider.value = health;
        }
    }

    private void LateDestroy()
    {
        Destroy(gameObject);
    }
}
