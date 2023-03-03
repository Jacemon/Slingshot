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
        public State state;
        [Space]
        public float flightTime = 1.0f;
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

        public void DoRandomForce()
        {
            var randomVelocity = Random.Range(randomVelocityRange.x, randomVelocityRange.y);
            // Направление в первых двух четвертях единичной окружности
            Vector2 randomDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 1.0f));
            _rb.AddForce(randomDirection * randomVelocity);
        }

        public void Shoot(Vector2 force)
        {
            GlobalEventManager.OnProjectileThrow?.Invoke(this);
        
            gameObject.layer = LayerMask.NameToLayer("Middle");
        
            _rb.isKinematic = false;
            _rb.velocity = force; // or AddForce() but it's requires NORMAL mass;
            _collider2D.enabled = false;
            _follower.enabled = false;

            StartCoroutine(nameof(WaitForHit));
        }

        private IEnumerator WaitForHit()
        {
            state = State.InFlight;
            yield return new WaitForSecondsRealtime(flightTime);
            state = State.InCalm;

            _collider2D.enabled = true;
            Debug.Log($"{projectileName} can be stuck in target");
        
            yield return new WaitForSecondsRealtime(0.1f);

            if (state != State.InHit)
            {
                state = State.InFlight;
            }

            var thisGameObject = gameObject;
            thisGameObject.layer = LayerMask.NameToLayer("Back");

            yield return new WaitForSecondsRealtime(TimeBeforeDestroy);

            Destroy(thisGameObject);
        }
    }
}
