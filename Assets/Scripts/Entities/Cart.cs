using System;
using Entities.Levels;
using Entities.Targets;
using Managers;
using Tools;
using Tools.Follower;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Timer))]
    [RequireComponent(typeof(PathFollower))]
    public class Cart : MonoBehaviour
    {
        [Header("Settings")] 
        public string cartName;
        [Space]
        [Tooltip("Extra time after the projectile flight time")]
        public float fallTime;
        [Header("Special settings")] 
        public AudioSource cartMoving;
        public AudioSource cartHit;

        private const float AnimationSpeedCoefficient = 0.5f;
        private const float TargetFadeTime = 0.3f;

        private Timer _timer;
        private PathFollower _pathFollower;
        private ParticleSystem _particleSystem;
        private Animator _animator;
        private static readonly int IsMovingLeft = Animator.StringToHash("IsMovingLeft");

        private void Awake()
        {
            _timer = GetComponent<Timer>();
            _pathFollower = GetComponent<PathFollower>();
            _animator = GetComponent<Animator>();
            _animator.speed = _pathFollower.velocity
                .Evaluate(Time.time, Random.Range(0.0f, 1.0f)) * AnimationSpeedCoefficient;
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            GlobalEventManager.OnProjectileThrown += AddTimerDelay;
            GlobalEventManager.OnLevelLoaded += CheckLevel;
            _timer.OnTimerDone += ResumeCart;
            _pathFollower.OnMovingLeft += MoveLeft;
            _pathFollower.OnMovingRight += MoveRight;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.OnProjectileThrown -= AddTimerDelay;
            GlobalEventManager.OnLevelLoaded -= CheckLevel;
            _timer.OnTimerDone -= ResumeCart;
            _pathFollower.OnMovingLeft -= MoveLeft;
            _pathFollower.OnMovingRight -= MoveRight;
        }

        private void CheckLevel(Level level)
        {
            if (level is BossLevel)
            {
                PauseCart();
                _timer.enabled = false;
            }
            else
            {
                ResumeCart();
                _timer.enabled = true;
            }
        }
        
        private void MoveLeft()
        {
            _animator.SetBool(IsMovingLeft, true);
        }
        
        private void MoveRight()
        {
            _animator.SetBool(IsMovingLeft, false);
        }
        
        private void PauseCart()
        {
            cartMoving.mute = true;
            _animator.enabled = false;
            _pathFollower.Pause();
            
            Debug.Log($"Cart {cartName} paused");
        }

        private void ResumeCart()
        {
            cartMoving.mute = false;
            _animator.enabled = true;
            _pathFollower.Resume();
            
            Debug.Log($"Cart {cartName} resumed");
        }

        private void AddTimerDelay(Projectile projectile)
        {
            PauseCart();
            
            _timer.SetBiggerDelay(projectile.flightTime + fallTime);
            _timer.timerOn = true;
            
            Debug.Log($"Try set timer to {projectile.flightTime + fallTime}s");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"{collision.gameObject.name} collided with {cartName}");
            
            if (!collision.gameObject.TryGetComponent(out Target target))
            {
                return;
            }
            
            GlobalEventManager.OnTargetHitCart?.Invoke(target);
            MoneyManager.DepositMoney(target.money);

            _particleSystem.Play();
            cartHit.Play();
            
            // Destroy target
            if (target.TryGetComponent(out Collider2D targetCollider) && 
                target.TryGetComponent(out Rigidbody2D targetRigidbody))
            {
                targetCollider.enabled = false;
                targetRigidbody.velocity = Vector2.zero;
                targetRigidbody.angularVelocity = 0;
                targetRigidbody.isKinematic = true;
            }
            target.GetDamage(0);
            target.LateDestroy(TargetFadeTime);
        }
    }
}
