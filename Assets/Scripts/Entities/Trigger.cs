using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Animator))]
    public class Trigger : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Play = Animator.StringToHash("Play");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _animator.SetTrigger(Play);
            
            Debug.Log($"{name} triggered");
        }
    }
}