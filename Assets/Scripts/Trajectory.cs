using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{

    public int lineCount;

    LineRenderer lineRenderer;
    private Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = lineCount + 1;
    }

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = projectile.StartPosition - 
            new Vector2(projectile.transform.position.x, 
            projectile.transform.position.y);
        direction.Normalize();

        var vx = projectile.Velocity * direction.x;
        var vy = projectile.Velocity * direction.y;

        int i;
        float t;
        float flyTime = projectile.FlyTime;
        float dt = flyTime / lineCount;
        for (i = 0, t = 0.0f; i <= lineCount; i++, t+= dt)
        {
            var nextPos = new Vector2(vx * t, vy * t - (-Physics2D.gravity.y * t * t) / 2);
            lineRenderer.SetPosition(i, (Vector2)transform.position + nextPos);
        }
    }
}
