using Managers;
using Tools;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Timer))]
    public class Cart : MonoBehaviour
    {
        [Header("Settings")] 
        public string cartName = "None";
        [Space] 
        public Vector2[] positions;
        public float velocity;
        [Header("Current parameters")]
        [SerializeField] 
        private int positionIndex;

        private const float TargetFadeTime = 0.3f;
        private const float ErrorRate = 0.1f;
        private const int ParticlesRate = 20;

        private AudioSource _audioSource;
        private Timer _timer;
    
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.enabled = true;
            _timer = GetComponent<Timer>();
        }

        private void OnEnable()
        {
            GlobalEventManager.onProjectileThrow += AddTimerDelay;
        }
        
        private void OnDisable()
        {
            GlobalEventManager.onProjectileThrow -= AddTimerDelay;
        }

        private void FixedUpdate()
        {
            if (!_timer.timerDone)
            {
                _audioSource.enabled = false;
                return;
            }
        
            _audioSource.enabled = true;
        
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, positions[positionIndex], velocity);
            if (Vector2.Distance(transform.localPosition, positions[positionIndex]) < ErrorRate)
            {
                NextPosition();
            }
        }

        private void AddTimerDelay(Projectile projectile)
        {
            const float extraDelay = 1.0f;
            
            _timer.SetBiggerDelay(projectile.flightTime + extraDelay);
            _timer.timerOn = true;
            Debug.Log($"Try set timer to {projectile.flightTime + extraDelay}s");
        }
        
        private void NextPosition()
        {
            positionIndex++;
            if (positionIndex == positions.Length)
            {
                positionIndex = 0;
            }
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
