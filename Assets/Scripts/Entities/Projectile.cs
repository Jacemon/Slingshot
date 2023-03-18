using System.Collections;
using DG.Tweening;
using Managers;
using Tools;
using Tools.Follower;
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
        public ParticleSystem.MinMaxCurve minMaxVelocity;
        public ParticleSystem.MinMaxCurve minMaxAngularVelocity;
        [Space]
        public bool inPick;

        private Rigidbody2D _rb;
        private Collider2D _collider2D;
        private MouseFollower _mouseFollower;
        private Follower _follower;
        
        private const float AppearTime = 0.5f;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.isKinematic = true;
            _collider2D = GetComponent<Collider2D>();
            _mouseFollower = GetComponent<MouseFollower>();
            _mouseFollower.enabled = false;
            _follower = GetComponent<Follower>();
            _follower.enabled = true;

            Debug.Log($"{projectileName}:{level} was spawned");
        }

        private void OnEnable()
        {
            damage = damageCurve.ForceEvaluate(level);
            
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, AppearTime).SetEase(Ease.OutElastic);
        }
        
        private void OnDestroy()
        {
            transform.DOKill();
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null || !collision.gameObject.TryGetComponent(out Target target))
            {
                return;
            }
            target.GetDamage(damage);
            
            // Random angular and regular velocity
            var randomVelocity = minMaxVelocity.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
            var direction = transform.position - target.transform.position;
            _rb.velocity = randomVelocity * direction;
            _rb.angularVelocity = minMaxAngularVelocity.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
        }

        private void OnMouseDown()
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;
            _mouseFollower.enabled = inPick = true;
            _follower.enabled = false;
        }

        private void OnMouseUp()
        {
            _mouseFollower.enabled = inPick = false;
            _follower.enabled = true;
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
            _rb.angularVelocity = minMaxAngularVelocity.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
            _collider2D.enabled = false;
            _follower.enabled = false;
            
            transform.DOScale(new Vector2(finalScale, finalScale), flightTime).SetEase(Ease.OutSine);
            yield return new WaitForSecondsRealtime(flightTime);

            _collider2D.enabled = true;
            Debug.Log($"{projectileName} can be stuck in target");
        
            yield return new WaitForSecondsRealtime(stuckTime);

            _collider2D.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Back");
            
            transform.DOScale(Vector2.zero, flightTime)
                .SetEase(Ease.OutSine)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}
