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
        private static readonly int Jump = Animator.StringToHash("Jump");

        private Sequence _sequence;

        protected override void OnEnable()
        {
            base.OnEnable();

            OnStageChanged += ClearTween;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            OnStageChanged -= ClearTween;
            ClearTween();
        }

        public void ClearTween()
        {
            transform.DOKill();
            _sequence.Kill();
        }
        
        public void JumpStage()
        {
            Animator.speed = 1 / jumpTime;
            
            var jumpY = transform
                .DOMoveY(
                    transform.position.y + jumpHeight,
                    jumpTime / 2)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                    transform
                        .DOMoveY(
                            transform.position.y - jumpHeight,
                            jumpTime / 2)
                        .SetEase(Ease.OutSine)
                );
            
            _sequence = DOTween.Sequence().SetLoops(-1);

            _sequence.AppendCallback(() => Animator.SetTrigger(Jump));
            _sequence.AppendCallback(() =>
                {
                    transform.DOMoveX(
                        Random.Range(xRange.x, xRange.y),
                        jumpTime);
                }
            );
            _sequence.Append(jumpY);
            _sequence.AppendInterval(waitTime);
        }
    }
}