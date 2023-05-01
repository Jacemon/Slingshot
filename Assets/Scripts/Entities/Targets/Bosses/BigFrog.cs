using DG.Tweening;
using UnityEngine;

namespace Entities.Targets.Bosses
{
    public class BigFrog : BossTarget
    {
        [Header("BigFrog Settings")]
        public Vector2 xRange = new(-2, 2);
        public float jumpHeight = 20f;
        public float jumpTime;
        public float jumpWaitTime;
        [Space] 
        public float hideHeight = 5f;
        public float hideTime;
        public float hideWaitTime;
        
        private static readonly int Jump = Animator.StringToHash("Jump");

        private Collider2D _collider2d;
        
        private Sequence _sequence;
        private Vector3 _startPosition;
        
        protected override void Awake()
        {
            base.Awake();

            _startPosition = transform.position;
            _collider2d = GetComponent<Collider2D>();
        }

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
            transform.position = _startPosition;
        }
        
        public void JumpStage()
        {
            Animator.speed = 1 / jumpTime;
            
            var positionY = transform.position.y;

            _sequence = DOTween.Sequence().SetLoops(-1);
            
            _sequence.AppendCallback(() => Animator.SetTrigger(Jump));
            _sequence.AppendCallback(() =>
                {
                    transform.DOMoveX(
                        Random.Range(xRange.x, xRange.y),
                        jumpTime); // Random jump X
                }
            );
            _sequence.Append(
                transform
                    .DOMoveY(
                        positionY + jumpHeight,
                        jumpTime / 2)
                    .SetEase(Ease.InSine) // Jump up
            );
            _sequence.Append(
                transform
                    .DOMoveY(
                        positionY,
                        jumpTime / 2)
                    .SetEase(Ease.OutSine) // Jump down
            );
            _sequence.AppendInterval(jumpWaitTime);
        }

        public void HideStage()
        {
            var positionY = transform.position.y;

            transform.DOMoveZ(5, 0);
            
            _sequence = DOTween.Sequence().SetLoops(-1);

            _sequence.AppendCallback(() =>
                {
                    transform.DOMoveX(
                        Random.Range(xRange.x, xRange.y),
                        hideTime);
                }
            );
            _sequence.AppendCallback(() => _collider2d.enabled = false);
            _sequence.Append(
                transform
                    .DOMoveY(
                        positionY - hideHeight,
                        hideTime / 2)
                    .SetEase(Ease.InSine) // Hide
            );
            _sequence.Append(
                transform
                    .DOMoveY(
                        positionY,
                        hideTime / 2)
                    .SetEase(Ease.OutSine) // Show
            );
            _sequence.AppendCallback(() => _collider2d.enabled = true);
            _sequence.AppendInterval(hideWaitTime);
        }
    }
}