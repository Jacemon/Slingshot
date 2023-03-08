using System.Collections;
using Managers;
using Tools;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Follower))]
    [RequireComponent(typeof(MouseFollower))]
    public class Projectile : MonoBehaviour
    {
        [Header("Settings")]
        public string projectileName;
        [Space]
        public int level;
        [Space]
        public int damage;
        [Space]
        [Header("Special settings")]
        public IntLinearCurve damageCurve;
        [Space]
        public State state; // TODO: remake to State pattern
        [Space]
        public float flightTime = 1.0f;
        public float stuckTime = 0.1f;
        public float finalScale = 0.3f;
        public Vector2 randomVelocityRange = new (1, 3);
        
        private float _scaleVelocity;

        private Rigidbody2D _rb;
        private Collider2D _collider2D;
        private MouseFollower _mouseFollower;
        private Follower _follower;

        private const float TimeBeforeDestroy = 4f;
    
        public enum State
        {
            InCalm,
            InPick,
            InPouch,
            InFlight,
            InHit
        }
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.isKinematic = true;
            _collider2D = GetComponent<Collider2D>();
            _mouseFollower = GetComponent<MouseFollower>();
            _mouseFollower.enabled = false;
            _follower = GetComponent<Follower>();
            _follower.enabled = true;

            // Расчёт скорости уменьшения снаряда во время полёта
            _scaleVelocity = (1 - finalScale) / flightTime * Time.fixedDeltaTime;

            damage = damageCurve.ForceEvaluate(level);
            
            Debug.Log($"{projectileName}:{level} was spawned");
        }

        private void FixedUpdate()
        {
            // Уменьшение снаряда во время полёта
            if (state != State.InFlight)
            {
                return;
            }
        
            var newScale = _rb.transform.localScale - new Vector3(
                _scaleVelocity,
                _scaleVelocity,
                0);
            if (newScale.x > 0 || newScale.y > 0)
            {
                _rb.transform.localScale = newScale;
            }
            else
            {
                _rb.transform.localScale = Vector2.zero;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null || !collision.gameObject.TryGetComponent(out Target target))
            {
                return;
            }
            target.GetDamage(damage);
            state = State.InHit;
            
            // Random force
            var randomVelocity = Random.Range(randomVelocityRange.x, randomVelocityRange.y);
            var direction = transform.position - target.transform.position;
            _rb.velocity = randomVelocity * direction;
        }

        private void OnMouseDown()
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;
            _mouseFollower.enabled = true;
            _follower.enabled = false;
        
            state = State.InPick;
        }

        private void OnMouseUp()
        {
            _mouseFollower.enabled = false;
            _follower.enabled = true;
        
            state = State.InCalm;
        }

        public void Shoot(Vector2 force)
        {
            StartCoroutine(nameof(ShootCoroutine), force);
        }

        private IEnumerator ShootCoroutine(Vector2 force)
        {
            GlobalEventManager.onProjectileThrow?.Invoke(this);
        
            gameObject.layer = LayerMask.NameToLayer("Middle");
        
            _rb.isKinematic = false;
            _rb.velocity = force;
            _collider2D.enabled = false;
            _follower.enabled = false;
            
            state = State.InFlight;
            yield return new WaitForSecondsRealtime(flightTime);
            state = State.InCalm;

            _collider2D.enabled = true;
            Debug.Log($"{projectileName} can be stuck in target");
        
            yield return new WaitForSecondsRealtime(stuckTime);

            if (state != State.InHit)
            {
                state = State.InFlight;
            }

            gameObject.layer = LayerMask.NameToLayer("Back");

            yield return new WaitForSecondsRealtime(TimeBeforeDestroy);

            Destroy(gameObject);
        }
    }
}
