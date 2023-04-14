using DG.Tweening;
using UnityEngine;

namespace Entities.Targets.Bosses
{
    public class BigFrog : BossTarget
    {
        [Header("BigFrog Settings")]
        public Vector2 xRange = new(-2, 2);
        public float jumpHeight = 20f;
        [Space] 
        public float jumpTime;
        public float waitTime;
        
        public void Jump()
        {
            Animator.speed = 1 / jumpTime;
            
            Animator.SetTrigger("Jump");

            DOVirtual.DelayedCall(Animator.GetCurrentAnimatorStateInfo(0).length, () => Animator.enabled = false);
            DOVirtual.DelayedCall(Animator.GetCurrentAnimatorStateInfo(0).length + waitTime, () => Animator.enabled = true);
            
            transform.DOMoveX(
                Random.Range(xRange.x, xRange.y), 
                jumpTime);
            transform
                .DOMoveY(
                    transform.position.y + jumpHeight, 
                    jumpTime / 2)
                .SetEase(Ease.InOutSine)
                .OnComplete( () =>
                    transform
                        .DOMoveY(
                            transform.position.y - jumpHeight, 
                            jumpTime / 2)
                );
        }
    }
}