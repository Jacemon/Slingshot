using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Animator))]
    public class Trigger : MonoBehaviour
    {
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _animator.enabled = true;
            
            Debug.Log($"{name} triggered");
        }
    }
}