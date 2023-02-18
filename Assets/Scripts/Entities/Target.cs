using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class Target : MonoBehaviour
    {
        [Header("Settings")] 
        public string targetName = "None";
        [Space]
        public int health;
        public int maxHealth = 3;
        public int points = 1;
    
        private Slider _slider;
        private Canvas _healthBar;

        private const float TimeBeforeDestroy = 4f;
        private const float ShotColliderScale = 0.6f;

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
            var newHealth = health - damage;

            if (newHealth <= 0)
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
            else if (newHealth < maxHealth)
            {
                Debug.Log($"{targetName} get {damage} damage ({newHealth}/{maxHealth})");
                health = newHealth;
                
                _healthBar.enabled = true;
                _slider.value = health;
            }
            GetComponent<ParticleSystem>().Play();
        }

        private void LateDestroy()
        {
            Destroy(gameObject);
        }
    }
}
