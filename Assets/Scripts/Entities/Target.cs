using DG.Tweening;
using Managers;
using Tools;
using UnityEngine;

namespace Entities
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
        public ParticleSystem.MinMaxCurve minMaxPitch;
        [Space] 
        public float endScale = 1;
        
        private ParticleSystem _particleSystem;
        private Animator _animator;
        private static readonly int Hit = Animator.StringToHash("Hit");

        private const float AppearTime = 1.5f;
        private const float TimeBeforeDestroy = 2f;
        private const float ShotColliderScale = 0.6f;
        
        private void Awake()
        {
            health = maxHealth = maxHealthCurve.ForceEvaluate(level);
            money = moneyCurve.ForceEvaluate(level);

            healthBar.Health = healthBar.MaxHealth = maxHealth;
            
            _animator = GetComponent<Animator>();
            _particleSystem = GetComponent<ParticleSystem>();

            Debug.Log($"{targetName}:{level} was spawned");
        }

        public void OnEnable()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(endScale, AppearTime).SetEase(Ease.OutExpo);
        }
        
        private void OnDisable()
        {
            transform.DOKill();
        }
        
        public void GetDamage(int damage)
        {
            health -= damage;
            healthBar.Health -= damage;
            Debug.Log($"{targetName} get {damage} damage ({health}/{maxHealth})");

            targetHit.pitch = minMaxPitch.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
            targetHit.Play();
            
            _particleSystem.Play();
            _animator.SetTrigger(Hit);

            if (health <= 0)
            {
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

            GlobalEventManager.onTargetGetDamage?.Invoke(this);
        }

        public void LateDestroy(float time = TimeBeforeDestroy)
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
