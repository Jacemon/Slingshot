using DG.Tweening;
using Managers;
using Tools;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Timer))]
    public class Cart : MonoBehaviour
    {
        [Header("Settings")] 
        public string cartName;
        [Space] 
        public Vector2[] points;
        public float velocity;
        [Tooltip("Extra time after the projectile flight time")]
        public float fallTime;
        [Header("Special settings")] 
        public AudioSource cartMoving;
        public AudioSource cartHit;

        private const float AnimationSpeedCoefficient = 0.5f;
        private const float TargetFadeTime = 0.3f;
        private const int ParticlesRate = 20;

        private Timer _timer;
        private Sequence _sequence;
        private Animator _animator;
        private static readonly int IsMovingLeft = Animator.StringToHash("IsMovingLeft");

        private void Awake()
        {
            _timer = GetComponent<Timer>();
            _animator = GetComponent<Animator>();
            _animator.speed = velocity * AnimationSpeedCoefficient;
            Tween();
        }

        private void OnEnable()
        {
            GlobalEventManager.onProjectileThrow += AddTimerDelay;
            _timer.onTimerDone += ResumeCart;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.onProjectileThrow -= AddTimerDelay;
            _timer.onTimerDone -= ResumeCart;
        }

        private void OnDestroy()
        {
            transform.DOKill();
            _sequence.Kill();
        }

        private void PauseCart()
        {
            cartMoving.mute = true;
            _animator.enabled = false;
            _sequence.Pause();
        }

        private void ResumeCart()
        {
            cartMoving.mute = false;
            _animator.enabled = true;
            _sequence.Play();
        }

        private void Tween()
        {
            transform.position = points[^1];

            _sequence = DOTween.Sequence().SetLoops(-1);

            _sequence.AppendCallback(() => _animator.SetBool(IsMovingLeft, points[^1].x < points[0].x));
            _sequence.Append(transform
                .DOMove(points[0], Vector2.Distance(points[^1], points[0]) / velocity)
                .SetEase(Ease.Linear)
            );
            for (var i = 1; i < points.Length; i++)
            {
                var startPoint = points[i - 1];
                var endPoint = points[i];
                
                _sequence.AppendCallback(() => _animator.SetBool(IsMovingLeft, endPoint.x < startPoint.x));
                _sequence.Append(transform
                    .DOMove(endPoint, Vector2.Distance(startPoint, endPoint) / velocity)
                    .SetEase(Ease.Linear)
                );
            }
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
        
            var target = collision.gameObject.GetComponent<Target>();
            if (target == null)
            {
                return;
            }
            if (TryGetComponent(out ParticleSystem particles))
            {
                particles.Emit(ParticlesRate);
                cartHit.Play();
            }
            
            GlobalEventManager.onTargetHitCart?.Invoke(target);
            
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
