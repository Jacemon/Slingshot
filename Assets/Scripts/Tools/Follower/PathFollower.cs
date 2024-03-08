using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Tools.Interfaces;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

namespace Tools.Follower
{
    public class PathFollower : MonoBehaviour, IReloadable
    {
        public enum PointType
        {
            Vector2,
            Vector3,
            GameObject
        }
        
        public PointType pointType;
        public List<Vector2> vector2Points = new();
        public List<Vector3> vector3Points = new();
        public List<GameObject> gameObjectPoints = new();
        
        public MinMaxCurve velocity = 1;
        [Space]
        [Tooltip("Number of cycles to play (-1 for infinite)")]
        public int loops = -1;
        public bool clamp = true;
        
        private ParticleSystem _particleSystem;
        private Sequence _sequence;
        
        public Action MovingLeft;
        public Action MovingRight;

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
            PointsCast();
            
            Gizmos.color = Color.green;

            if (vector3Points.Count == 0) return;

            var prevPoint = vector3Points[0];
            for (var i = 1; i < vector3Points.Count; i++)
            {
                Gizmos.DrawLine(prevPoint, vector3Points[i]);
                prevPoint = vector3Points[i];
            }

            Gizmos.DrawLine(vector3Points[0], vector3Points[^1]);
        }

        private void PointsCast()
        {
            switch (pointType)
            {
                case PointType.Vector2:
                    vector3Points = new List<Vector3>();
                    foreach (var vector2Point in vector2Points)
                    {
                        vector3Points.Add(vector2Point);
                    }
                    break;
                case PointType.GameObject:
                    vector3Points = new List<Vector3>();
                    foreach (var gameObjectPoint in gameObjectPoints)
                    {
                        vector3Points.Add(gameObjectPoint.transform.position);
                    }
                    break;
            }
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
            (CheckDirection(startPoint, endPoint) ? MovingLeft : MovingRight)?.Invoke();
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
            PointsCast();
            
            if (vector3Points.Count == 0) return;

            transform.position = vector3Points[0];

            _sequence = DOTween.Sequence().SetLoops(loops);

            for (var i = 1; i < vector3Points.Count; i++)
            {
                var startPoint = vector3Points[i - 1];
                var endPoint = vector3Points[i];

                _sequence.AppendCallback(() => CheckDirectionInvoke(startPoint, endPoint));
                _sequence.Append(transform
                    .DOMove(endPoint, Vector2.Distance(startPoint, endPoint)
                                      / velocity.Evaluate(Time.time, Random.Range(0.0f, 1.0f)))
                    .SetEase(Ease.Linear)
                );
            }

            if (!clamp) return;
            _sequence.AppendCallback(() => CheckDirectionInvoke(vector3Points[^1], vector3Points[0]));
            _sequence.Append(transform
                .DOMove(vector3Points[0], Vector2.Distance(vector3Points[^1], vector3Points[0])
                                   / velocity.Evaluate(Time.time, Random.Range(0.0f, 1.0f)))
                .SetEase(Ease.Linear)
            );
        }
    }
}