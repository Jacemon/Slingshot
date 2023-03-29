using System;
using DG.Tweening;
using Tools;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEngine.ParticleSystem;

namespace Entities.Targets
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(Animator))]
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
        [Space] 
        public IntHealthBar healthBar; 
        public AudioSource targetHit;
        public MinMaxCurve minMaxPitch;
        [Space] 
        public float appearScale = 1;
        public float appearTime = 1.5f;
        public float destroyTime = 2f;

        public Action OnHealthChanged;
        
        private ParticleSystem _particleSystem;
        private Animator _animator;
        private static readonly int Hit = Animator.StringToHash("Hit");

        private const float ShotColliderScale = 0.6f;
        
        protected virtual void Awake()
        {
            health = maxHealth = maxHealthCurve.ForceEvaluate(level);
            money = moneyCurve.ForceEvaluate(level);

            healthBar.Health = healthBar.MaxHealth = maxHealth;
            
            _animator = GetComponent<Animator>();
            _particleSystem = GetComponent<ParticleSystem>();

            Debug.Log($"{targetName}:{level} was spawned");
        }

        protected virtual void OnEnable()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(appearScale, appearTime).SetEase(Ease.OutExpo);

            OnHealthChanged += CheckHealth;
        }
        
        protected virtual void OnDisable()
        {
            transform.DOKill();
            
            OnHealthChanged -= CheckHealth;
        }
        
        public virtual void GetDamage(int damage)
        {
            health -= damage;
            healthBar.Health -= damage;
            Debug.Log($"{targetName} get {damage} damage ({health}/{maxHealth})");

            targetHit.pitch = minMaxPitch.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
            targetHit.Play();
            
            _particleSystem.Play();
            _animator.SetTrigger(Hit);

            OnHealthChanged?.Invoke();
        }

        private void CheckHealth()
        {
            if (health > 0) return;
            
            GetComponent<Rigidbody2D>().isKinematic = false;
            Debug.Log($"{targetName} shot down");
            gameObject.layer = LayerMask.NameToLayer("RearMiddle");
                
            object dummy = GetComponent<Collider2D>() switch
            {
                CircleCollider2D circle => circle.radius *= ShotColliderScale,
                CapsuleCollider2D capsule => capsule.size *= ShotColliderScale,
                _ => null
            };
        }

        public void LateDestroy()
        {
            LateDestroy(destroyTime);
        }
        public void LateDestroy(float time)
        {
            if (transform != null)
            {
                transform.DOScale(Vector2.zero, time)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(gameObject));
            }
        }
    }
}