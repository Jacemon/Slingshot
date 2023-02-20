using Tools;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MouseFollower))]
    [RequireComponent(typeof(Trajectory))]
    public class Pouch : MonoBehaviour
    {
        [Header("Settings")]
        public float throwSpeed = 7.0f; 
        public GameObject throwPointAnchor;
        public float throwPointOffset = 0.7f;

        [Header("Current parameters")] 
        [SerializeField]
        private bool pouchFill;
        public Projectile projectile;
        public float velocity;
        private Vector2 _direction;
    
        private Rigidbody2D _rb;
        private Collider2D _collider2D;
        private MouseFollower _mouseFollower;
    
        private Vector2 _throwPointAnchor;
        private Trajectory _trajectory;
        private SpringJoint2D[] _springJoints2D;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _mouseFollower = GetComponent<MouseFollower>();
            _mouseFollower.enabled = false;
        
            _trajectory = GetComponent<Trajectory>();
            _springJoints2D = GetComponents<SpringJoint2D>();
        }

        private void Update()
        {
            _throwPointAnchor = throwPointAnchor.transform.position;
        
            if (!pouchFill)
            {
                return;
            }
        
            if (projectile.state == Projectile.State.InPouch)
            {
                _mouseFollower.enabled = true;
            
                // Расчёт угла поворота
                Vector2 currentPosition = transform.position;
                _direction = _throwPointAnchor - new Vector2(currentPosition.x, currentPosition.y);
                _direction.Normalize();
                float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90.0f;
                // И поворот ложи на этот угол
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Расчёт скорости с которой он может полететь
                velocity = Vector2.Distance(currentPosition, _throwPointAnchor) * throwSpeed;
            
                // Установка значений для расчёта траектории
                _trajectory.velocity = velocity;
                _trajectory.flightTime = projectile.flightTime;
                _trajectory.startPosition = currentPosition;
                _trajectory.anchorPosition = _throwPointAnchor;
            }
            else
            {
                _mouseFollower.enabled = false;
                EmptyPouch();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null)
            {
                return;
            }

            if (collision.gameObject.CompareTag("Projectile"))
            {
                FillPouch(collision.gameObject.GetComponent<Projectile>());
            }
        }

        private void FillPouch(Projectile projectileToFill)
        {
            if (pouchFill || projectileToFill.state != Projectile.State.InPick)
            {
                return;
            }
        
            pouchFill = true;

            GetComponent<Collider2D>().enabled = false;
        
            projectile = projectileToFill;
        
            var projectileTransform = projectile.transform;
            projectileTransform.parent = transform;
            projectileTransform.localPosition = Vector3.zero;

            projectile.state = Projectile.State.InPouch;

            projectile.GetComponent<MouseFollower>().enabled = false;

            _rb.isKinematic = true;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;

            _trajectory.isDrawing = true;
        
            _springJoints2D[0].enabled = false;
            _springJoints2D[1].enabled = false;
        }

        private void EmptyPouch()
        {
            pouchFill = false;
        
            projectile.state = Projectile.State.InCalm;
            if (transform.position.y < _throwPointAnchor.y - throwPointOffset)
            {
                _rb.velocity = _direction * velocity; // or AddForce() but it's requires NORMAL mass;
                projectile.Shoot(_direction * velocity);
                projectile.state = Projectile.State.InFlight;
            }
        
            _collider2D.enabled = true;
        
            projectile.transform.parent = null;
            projectile = null;
        
            _rb.isKinematic = false;

            _trajectory.isDrawing = false;
        
            _springJoints2D[0].enabled = true;
            _springJoints2D[1].enabled = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_throwPointAnchor, 0.1f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector2(_throwPointAnchor.x, _throwPointAnchor.y - throwPointOffset), 0.1f);
        }
    }
}
