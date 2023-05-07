using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(LineRenderer))]
    public class StaticTrajectory : MonoBehaviour
    {
        [Header("Settings")]
        public Vector2 startPosition;
        public Vector2 anchorPosition;
        public float velocity;
        public float flightTime;
        [Header("Special settings")]
        public int lineCount = 20;

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = lineCount + 1;
        }

        private void Update()
        {
            var direction = anchorPosition - startPosition;
            direction.Normalize();

            var vx = velocity * direction.x;
            var vy = velocity * direction.y;

            int i;
            float t;
            var time = flightTime;
            var dt = time / lineCount;
            for (i = 0, t = 0.0f; i <= lineCount; i++, t += dt)
            {
                var nextPos = new Vector3(vx * t, vy * t + Physics2D.gravity.y * t * t / 2);
                _lineRenderer.SetPosition(i, transform.position + nextPos);
            }
        }

        private void OnEnable()
        {
            _lineRenderer.enabled = true;
        }

        private void OnDisable()
        {
            _lineRenderer.enabled = false;
        }
    }
}