using UnityEngine;

namespace Tools.Follower
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Follower : MonoBehaviour
    {
        public float dragSpeed = 20.0f;
        public Vector2 followPoint;

        private Rigidbody2D _rb;
    
        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            _rb.transform.position = Vector2.MoveTowards(
                _rb.transform.position,
                followPoint,
                Time.deltaTime * dragSpeed
            );
        }

        private void OnDrawGizmos()
        {
            if (!enabled)
            {
                return;
            }
        
            Gizmos.color = Color.green;
            Gizmos.DrawLine(followPoint, transform.position);
        }
    }
}