using Managers;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class Target : MonoBehaviour
    {
        [Header("Settings")] 
        public string targetName;
        [Space]
        public int level;
        [Space]
        public int money;
        public int maxHealth;
        public int health;
        [Space]
        [Header("Special settings")]
        public IntLinearCurve moneyCurve;
        public IntLinearCurve maxHealthCurve;
        
        private Slider _slider;
        private Canvas _healthBar;

        private const float TimeBeforeDestroy = 4f;
        private const float ShotColliderScale = 0.6f;

        private void Awake()
        {
            maxHealth = health = maxHealthCurve.ForceEvaluate(level);
            money = moneyCurve.ForceEvaluate(level);
            
            _healthBar = GetComponentInChildren<Canvas>();
            _healthBar.enabled = false;

            _slider = _healthBar.GetComponentInChildren<Slider>();
            _slider.maxValue = maxHealth;
            _slider.value = health;
            
            Debug.Log($"{targetName}:{level} was spawned");
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
            if (TryGetComponent(out ParticleSystem particles))
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
