using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(LineRenderer))]
    public class Trajectory : MonoBehaviour
    {
        [Header("Settings")]
        public Vector2 startPosition;
        public Vector2 anchorPosition;
        public float velocity;
        public float flightTime;
        [Header("Special settings")]
        public int lineCount = 20;
    
        private LineRenderer _lineRenderer;
        public bool isDrawing;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = lineCount + 1;
        }

        private void Update()
        {
            if (!isDrawing)
            {
                _lineRenderer.enabled = false;
                return;
            }
            _lineRenderer.enabled = true;

            if (_lineRenderer.positionCount != lineCount + 1)
            {
                _lineRenderer.positionCount = lineCount + 1;
            }

            TrajectoryCalculation();
        }

        private void TrajectoryCalculation()
        {
            var direction = anchorPosition - startPosition;
            direction.Normalize();

            var vx = velocity * direction.x;
            var vy = velocity * direction.y;

            int i;
            float t;
            var time = flightTime;
            var dt = time / lineCount;
            for (i = 0, t = 0.0f; i <= lineCount; i++, t+= dt)
            {
                var nextPos = new Vector3(vx * t, vy * t + Physics2D.gravity.y * t * t / 2);
                _lineRenderer.SetPosition(i, transform.position + nextPos);
            }
        }
    }
}
