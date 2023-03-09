using System.Collections;
using Managers;
using Tools;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Follower))]
    [RequireComponent(typeof(MouseFollower))]
    public class Projectile : MonoBehaviour
    {
        [Header("Settings")]
        public string projectileName;
        public int level;
        [Space]
        public int damage;
        [Header("Special settings")]
        public IntLinearCurve damageCurve;
        [Space]
        public float flightTime = 1.0f;
        public float stuckTime = 0.1f;
        public float finalScale = 0.3f;
        public Vector2 randomVelocityRange = new (1, 3);
        [Header("Current parameters")]
        public State state;

        private Rigidbody2D _rb;
        private Collider2D _collider2D;
        private MouseFollower _mouseFollower;
        private Follower _follower;

        private const float TimeBeforeDestroy = 2f;
        private const float AppearTime = 0.5f;
    
        public enum State
        {
            InCalm,
            InPick
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

            transform.localScale = Vector3.zero;
            transform.LeanScale(Vector3.one, AppearTime).setEaseOutElastic();
            
            damage = damageCurve.ForceEvaluate(level);
            
            Debug.Log($"{projectileName}:{level} was spawned");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null || !collision.gameObject.TryGetComponent(out Target target))
            {
                return;
            }
            target.GetDamage(damage);
            
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
            
            transform.LeanScale(new Vector2(finalScale, finalScale), flightTime).setEaseOutSine();
            yield return new WaitForSecondsRealtime(flightTime);

            _collider2D.enabled = true;
            Debug.Log($"{projectileName} can be stuck in target");
        
            yield return new WaitForSecondsRealtime(stuckTime);

            gameObject.layer = LayerMask.NameToLayer("Back");
            
            transform.LeanScale(Vector2.zero, flightTime).setEaseOutSine();
            yield return new WaitForSecondsRealtime(TimeBeforeDestroy);
            
            Destroy(gameObject);
        }
    }
}
