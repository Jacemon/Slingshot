using DG.Tweening;
using Managers;
using Tools;
using UnityEngine;
using UnityEngine.UI;

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
        public AudioSource targetHit;
        public ParticleSystem.MinMaxCurve minMaxPitch;
        
        private Slider _slider;
        private Canvas _healthBar;
        private ParticleSystem _particleSystem;
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

            _particleSystem = GetComponent<ParticleSystem>();

            Debug.Log($"{targetName}:{level} was spawned");
        }

        public void OnEnable()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, AppearTime).SetEase(Ease.OutExpo);
        }
        
        private void OnDestroy()
        {
            transform.DOKill();
        }
        
        public void GetDamage(int damage)
        {
            health -= damage;
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
            } 
            else if (health < maxHealth)
            {
                _healthBar.enabled = true;
                _slider.value = health;
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
