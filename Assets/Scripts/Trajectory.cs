using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [Header("Settings")]
    public int LineCount = 10;
    [Header("Not null settings")]
    public LineRenderer LineRenderer;

    private Projectile Projectile;

    private void Awake()
    {
        Projectile = GetComponent<Projectile>();
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.positionCount = LineCount + 1;
    }

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = Projectile.StartPosition - 
            new Vector2(Projectile.transform.position.x, 
            Projectile.transform.position.y);
        direction.Normalize();

        var vx = Projectile.Velocity * direction.x;
        var vy = Projectile.Velocity * direction.y;

        int i;
        float t;
        float flyTime = Projectile.FlightTime;
        float dt = flyTime / LineCount;
        for (i = 0, t = 0.0f; i <= LineCount; i++, t+= dt)
        {
            var nextPos = new Vector2(vx * t, vy * t - (-Physics2D.gravity.y * t * t) / 2);
            LineRenderer.SetPosition(i, (Vector2)transform.position + nextPos);
        }
    }
}
