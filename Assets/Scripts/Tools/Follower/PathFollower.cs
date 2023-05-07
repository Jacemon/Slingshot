using System;
using System.Collections.Generic;
using DG.Tweening;
using Tools.Interfaces;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

namespace Tools.Follower
{
    public class PathFollower : MonoBehaviour, IReloadable
    {
        public List<Vector2> points = new();
        public MinMaxCurve velocity = 1;
        [Space]
        [Tooltip("Number of cycles to play (-1 for infinite)")]
        public int loops = -1;
        public bool clamp = true;
        
        private ParticleSystem _particleSystem;
        private Sequence _sequence;

        public Action OnMovingLeft;
        public Action OnMovingRight;

        private void OnEnable()
        {
            Tween();
        }

        private void OnDisable()
        {
            transform.DOKill();
            _sequence.Kill();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            if (points.Count == 0) return;

            var prevPoint = points[0];
            for (var i = 1; i < points.Count; i++)
            {
                Gizmos.DrawLine(prevPoint, points[i]);
                prevPoint = points[i];
            }

            Gizmos.DrawLine(points[0], points[^1]);
        }

        public void Reload()
        {
            transform.DOKill();
            _sequence.Kill();
            Tween();
        }

        /// <returns>
        ///     true - left,
        ///     false - right
        /// </returns>
        public static bool CheckDirection(Vector2 startPoint, Vector2 endPoint)
        {
            return startPoint.x > endPoint.x;
        }

        private void CheckDirectionInvoke(Vector2 startPoint, Vector2 endPoint)
        {
            (CheckDirection(startPoint, endPoint) ? OnMovingLeft : OnMovingRight)?.Invoke();
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
            if (points.Count == 0) return;

            transform.position = points[0];

            _sequence = DOTween.Sequence().SetLoops(loops);

            for (var i = 1; i < points.Count; i++)
            {
                var startPoint = points[i - 1];
                var endPoint = points[i];

                _sequence.AppendCallback(() => CheckDirectionInvoke(startPoint, endPoint));
                _sequence.Append(transform
                    .DOMove(endPoint, Vector2.Distance(startPoint, endPoint)
                                      / velocity.Evaluate(Time.time, Random.Range(0.0f, 1.0f)))
                    .SetEase(Ease.Linear)
                );
            }

            if (!clamp) return;
            _sequence.AppendCallback(() => CheckDirectionInvoke(points[^1], points[0]));
            _sequence.Append(transform
                .DOMove(points[0], Vector2.Distance(points[^1], points[0])
                                   / velocity.Evaluate(Time.time, Random.Range(0.0f, 1.0f)))
                .SetEase(Ease.Linear)
            );
        }
    }
}