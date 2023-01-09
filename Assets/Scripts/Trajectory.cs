using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Trajectory : MonoBehaviour
{
    [Header("Settings")]
    public int lineCount = 10;
    
    private LineRenderer _lineRenderer;
    private Projectile _projectile;

    private void Awake()
    {
        _projectile = GetComponent<Projectile>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = lineCount + 1;
    }

    private void Update()
    {
        var projectilePosition = _projectile.transform.position;
        var direction = _projectile.startPosition - 
            new Vector2(projectilePosition.x, projectilePosition.y);
        direction.Normalize();

        var vx = _projectile.velocity * direction.x;
        var vy = _projectile.velocity * direction.y;

        int i;
        float t;
        float time = _projectile.flightTime;
        float dt = time / lineCount;
        for (i = 0, t = 0.0f; i <= lineCount; i++, t+= dt)
        {
            var nextPos = new Vector3(vx * t, vy * t + Physics2D.gravity.y * t * t / 2);
            _lineRenderer.SetPosition(i, transform.position + nextPos);
        }
    }
}
