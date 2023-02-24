using Managers;
using Tools;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Timer))]
    public class Cart : MonoBehaviour
    {
        [Header("Settings")] 
        public string cartName = "None";

        [Space] 
        public Vector2[] positions;
        public float velocity;

        [Header("Current parameters")]
        [SerializeField] private int positionIndex;

        private const float ErrorRate = 0.1f;
        private Timer _timer;
    
        private void Awake()
        {
            _timer = GetComponent<Timer>();
        }
    
        private void FixedUpdate()
        {
            if (!_timer.timerDone)
            {
                return;
            }
        
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, positions[positionIndex], velocity);
            if (Vector2.Distance(transform.localPosition, positions[positionIndex]) < ErrorRate)
            {
                NextPosition();
            }
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
            Debug.Log($"{collision.gameObject.tag} collided with {cartName}");
        
            var target = collision.gameObject.GetComponent<Target>();
            if (target == null)
            {
                return;
            }
            if (TryGetComponent<ParticleSystem>(out var particles))
            {
                particles.Emit(target.money);
            }
            
            GlobalEventManager.OnTargetHitCart?.Invoke(target);
        }
    }
}
