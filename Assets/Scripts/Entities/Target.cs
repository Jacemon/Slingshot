using Managers;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class Target : MonoBehaviour, IReloadable
    {
        [Header("Settings")] 
        public string targetName = "None";
        [Space]
        public int level;
        [Space]
        public int startHealth = 1;
        public int maxHealthMultiplier = 3;
        [Space]
        public int startMoney;
        public int moneyMultiplier;
        [Space]
        [Header("money = startMoney + moneyMultiplier * level")]
        public int money;
        [SerializeField]
        [Header("maxHealth = startHealth + healthMultiplier * level")]
        private int maxHealth;
        public int health;
        
        private Slider _slider;
        private Canvas _healthBar;

        private const float TimeBeforeDestroy = 4f;
        private const float ShotColliderScale = 0.6f;

        private void Awake()
        {
            _healthBar = GetComponentInChildren<Canvas>();
            _healthBar.enabled = false;
            
            _slider = _healthBar.GetComponentInChildren<Slider>();

            Reload();
        }

        public void Reload()
        {
            maxHealth = health = startHealth + maxHealthMultiplier * level;
            _slider.maxValue = maxHealth;
            _slider.value = health;

            money = startMoney + moneyMultiplier * level;
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
            
                switch (GetComponent<Collider2D>())
                {
                    case CircleCollider2D:
                        GetComponent<CircleCollider2D>().radius *= ShotColliderScale;
                        break;
                    case CapsuleCollider2D:
                        GetComponent<CapsuleCollider2D>().size *= ShotColliderScale;
                        break;
                }
            
                Invoke(nameof(LateDestroy), TimeBeforeDestroy);
            } 
            else if (health < maxHealth)
            {
                _healthBar.enabled = true;
                _slider.value = health;
            }
            if (TryGetComponent<ParticleSystem>(out var particles))
            {
                particles.Play();
            }
            
            GlobalEventManager.OnTargetGetDamage?.Invoke(this);
        }

        private void LateDestroy()
        {
            Destroy(gameObject);
        }
    }
}
