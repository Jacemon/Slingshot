using Managers;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [Header("Settings")] 
    public string targetName = "None";
    [Space]
    public int maxHealth = 3;
    public int points = 100;
    [Space] 
    //public ParticleSystem particleSystem;
    
    [SerializeField]
    private int health;
    
    private Slider _slider;
    private Canvas _healthBar;

    private const float TimeBeforeDestroy = 4f; 
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
    
    public void GetDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{targetName} get {damage} damage ({health}/{maxHealth})");
        
        if (health <= 0)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            Debug.Log($"{targetName} shot down");
            gameObject.layer = LayerMask.NameToLayer("Back");
            _healthBar.enabled = false;
            GetComponent<CircleCollider2D>().radius = ShotColliderRadius;

            Invoke(nameof(LateDestroy), TimeBeforeDestroy);
        } 
        else if (health < maxHealth)
        {
            _healthBar.enabled = true;
            _slider.value = health;
        }
        //GetComponent<ParticleSystem>().Emit(20);
        GetComponent<ParticleSystem>().Play();
    }

    private void LateDestroy()
    {
        Destroy(gameObject);
    }
}
