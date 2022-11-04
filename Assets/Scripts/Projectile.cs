using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool MouseDown = false;
    public float DragSpeed = 20.0f;
    public float FlyTime = 2.0f;
    public Vector2 StartPosition = new Vector2(0, -2);

    public float Velocity = 0.0f;
    private Vector2 Direction;

    public Pouch Pouch;

    private Rigidbody2D rb;
    private Rigidbody2D pouchRb;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        rb.isKinematic = false;
    }

    private void Update()
    {
        if (MouseDown)
        {
            // Плавно следовать за мышью
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.transform.position = Vector2.MoveTowards(
                rb.transform.position,
                new Vector2(mousePos.x, mousePos.y),
                Time.deltaTime * DragSpeed
            );

            if (Pouch != null)
            {
                // Расчёт угла поворота
                Direction = StartPosition - new Vector2(transform.position.x, transform.position.y);
                Direction.Normalize();
                var angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 90.0f;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                Velocity = Vector2.Distance(transform.position, StartPosition) * Pouch.ThrowSpeed;

                Pouch.transform.localPosition = new Vector3(0, -0.5f, 0);
                Pouch.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null)
        {
            return;
        }
        Pouch pouch = collision.gameObject.GetComponent<Pouch>();
        if (pouch == null || !pouch.CompareTag("Pouch") 
            || pouch.PouchFill == true || !MouseDown)
        {
            return;
        }

        Pouch = pouch;
        Pouch.PouchFill = true;
        Pouch.GetComponent<Collider2D>().enabled = false;
        Pouch.transform.parent = transform;
        pouchRb = pouch.GetComponent<Rigidbody2D>();
        pouchRb.isKinematic = true;

        lineRenderer.enabled = true;
    }

    private void OnMouseDown()
    {
        MouseDown = true;

        rb.isKinematic = true;

        rb.velocity = Vector2.zero;
        Velocity = 0;
        rb.angularVelocity = 0;
    }

    private void OnMouseUp()
    {
        MouseDown = false;

        rb.isKinematic = false;


        if (pouchRb != null)
        {
            pouchRb.isKinematic = false;
            pouchRb = null;
            GetComponent<Collider2D>().enabled = false;

            rb.velocity = Direction * Velocity; 

            Pouch.transform.parent = null;
            Pouch.GetComponent<Collider2D>().enabled = true;
            Pouch.PouchFill = false;
            Pouch = null;

            lineRenderer.enabled = false;

            StartCoroutine("WaitForHit");
        }
    }

    private IEnumerator WaitForHit()
    {
        transform.tag = "Throwed projectile";
        yield return new WaitForSecondsRealtime(FlyTime);

        GetComponent<Collider2D>().enabled = true;


        Debug.Log("can be stuck in");
    }
}
