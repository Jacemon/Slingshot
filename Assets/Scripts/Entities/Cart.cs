using Entities.Targets;
using Managers;
using Tools;
using Tools.Follower;
using UnityEngine;

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
            _animator.speed = _pathFollower.velocity * AnimationSpeedCoefficient;
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            GlobalEventManager.onProjectileThrow += AddTimerDelay;
            _timer.onTimerDone += ResumeCart;
            _pathFollower.onMovingLeft += MoveLeft;
            _pathFollower.onMovingRight += MoveRight;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.onProjectileThrow -= AddTimerDelay;
            _timer.onTimerDone -= ResumeCart;
            _pathFollower.onMovingLeft -= MoveLeft;
            _pathFollower.onMovingRight -= MoveRight;
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
        }

        private void ResumeCart()
        {
            cartMoving.mute = false;
            _animator.enabled = true;
            _pathFollower.Resume();
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
            
            GlobalEventManager.onTargetHitCart?.Invoke(target);
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
