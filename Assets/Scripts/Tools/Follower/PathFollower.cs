using System;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Tools.Follower
{
    public class PathFollower : MonoBehaviour
    {
        public Vector2[] points;
        public float velocity = 1;
        [Space]
        [Tooltip("Number of cycles to play (-1 for infinite)")]
        public int loops = -1;
        
        public Action onMovingLeft;
        public Action onMovingRight;

        private Sequence _sequence;
        private ParticleSystem _particleSystem;
        
        private void Awake()
        {
            Tween();
        }

        private void OnDisable()
        {
            transform.DOKill();
            _sequence.Kill();
        }
        
        private void CheckDirection(Vector2 startPoint, Vector2 endPoint)
        {
            (startPoint.x > endPoint.x ? onMovingLeft : onMovingRight)?.Invoke();
        }

        public void Pause()
        {
            _sequence.Pause();
        }

        public void Resume()
        {
            _sequence.Play();
        }
        
        private void Tween()
        {
            if (points.Length == 0) return;

            transform.position = points[^1];

            _sequence = DOTween.Sequence().SetLoops(loops);

            _sequence.AppendCallback(() => CheckDirection(points[^1], points[0]));
            _sequence.Append(transform
                .DOMove(points[0], Vector2.Distance(points[^1], points[0]) / velocity)
                .SetEase(Ease.Linear)
            );
            for (var i = 1; i < points.Length; i++)
            {
                var startPoint = points[i - 1];
                var endPoint = points[i];
                
                _sequence.AppendCallback(() => CheckDirection(startPoint, endPoint));
                _sequence.Append(transform
                    .DOMove(endPoint, Vector2.Distance(startPoint, endPoint) / velocity)
                    .SetEase(Ease.Linear)
                );
            }
        }
    }
}