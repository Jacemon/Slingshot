using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Trajectory : MonoBehaviour
{
    [Header("Settings")]
    public int lineCount = 10;
    
    private LineRenderer _lineRenderer;
    private Pouch _pouch;
    private bool _isDrawing;

    private void Awake()
    {
        _pouch = GetComponent<Pouch>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = lineCount + 1;
    }

    private void Update()
    {
        if (!_isDrawing)
        {
            return;
        }
        TrajectoryCalculation(_pouch.transform.position, _pouch.throwPointAnchor, 
            _pouch.velocity, _pouch.projectile.flightTime);
    }

    private void TrajectoryCalculation(Vector2 startPosition, Vector2 anchorPosition, float velocity, float flightTime)
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
    
    public void Draw()
    {
        _isDrawing = true;
        _lineRenderer.enabled = true;
    }
    
    public void NotDraw()
    {
        _isDrawing = false;
        _lineRenderer.enabled = false;
    }
}
