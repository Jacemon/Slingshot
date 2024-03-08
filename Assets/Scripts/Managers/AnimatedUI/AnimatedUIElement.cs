using UnityEngine;

namespace Managers.AnimatedUI
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedUIElement : MonoBehaviour
    {
        [Header("Base")]
        public float animationSpeed = 1f;
        
        protected Animator UIAnimator;

        private void Awake()
        {
            UIAnimator = GetComponent<Animator>();
        }
    }
}