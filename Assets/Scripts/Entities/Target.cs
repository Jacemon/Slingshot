using Managers;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Target : MonoBehaviour
    {
        [Header("Settings")] 
        public string targetName;
        public int level;
        [Space]
        public int money;
        public int maxHealth;
        public int health;
        [Header("Special settings")]
        public IntLinearCurve moneyCurve;
        public IntLinearCurve maxHealthCurve;
        
        private Slider _slider;
        private Canvas _healthBar;
        private Animator _animator;
        private static readonly int Hit = Animator.StringToHash("Hit");

        private const float AppearTime = 1.5f;
        private const float TimeBeforeDestroy = 2f;
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

            _animator = GetComponent<Animator>();
            
            transform.localScale = Vector3.zero;
            transform.LeanScale(Vector3.one, AppearTime).setEaseOutExpo();
            
            Debug.Log($"{targetName}:{level} was spawned");
        }

        public void GetDamage(int damage)
        {
            health -= damage;
            Debug.Log($"{targetName} get {damage} damage ({health}/{maxHealth})");
        
            _animator.SetTrigger(Hit);

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
            
                LateDestroy();
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
            
            GlobalEventManager.onTargetGetDamage?.Invoke(this);
        }

        public void LateDestroy(float time = TimeBeforeDestroy)
        {
            transform.LeanScale(Vector2.zero, time)
                .setEaseInBack()
                .setOnComplete(
                    () =>
                    {
                        Destroy(gameObject);
                    }
                );
        }
    }
}
