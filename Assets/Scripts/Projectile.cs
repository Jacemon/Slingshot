using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settigs")]
    public string ProjectileName = "None";
    public float DragSpeed = 20.0f;
    public float FlightTime = 2.0f;
    public float FinalScale = 0.3f;
    public Vector2 StartPosition = new Vector2(0, -2);

    [Header("Current parameters")]
    public float Velocity = 0.0f;
    public Pouch Pouch;

    private Vector2 Direction;
    private float ScaleVelocity;
    private bool InFlight = false;

    private bool MouseDown = false;
    private Rigidbody2D rb;
    private Rigidbody2D pouchRb;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        rb.isKinematic = false;

        ScaleVelocity = (1 - FinalScale) / FlightTime;
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

            // Пока снаряд не вылетел из рогатки
            if (Pouch != null)
            {
                // Расчёт угла поворота
                Direction = StartPosition - new Vector2(transform.position.x, transform.position.y);
                Direction.Normalize();
                var angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 90.0f;
                // И поворот рогатки на этот угол
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Расчёт скорости с которой он может полететь
                Velocity = Vector2.Distance(transform.position, StartPosition) * Pouch.ThrowSpeed;

                // Установка нормального положения снаряда в рогатке
                Pouch.transform.localPosition = new Vector3(0, -0.5f, 0);
                Pouch.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void FixedUpdate()
    {
        // Уменьшение снаряда во время полёта
        if (InFlight)
        {
            Vector3 newScale = rb.transform.localScale - new Vector3(
                ScaleVelocity * Time.deltaTime,
                ScaleVelocity * Time.deltaTime, 
                0);
            if (newScale.x > 0 || newScale.y > 0)
            {
                rb.transform.localScale = newScale;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null)
        {
            return;
        }
        // Если снаряд столкнулся не с рогаткой
        // или если рогатка уже занята
        // или если снаряд не был взят игроком
        // то выход
        Pouch pouch = collision.gameObject.GetComponent<Pouch>();
        if (pouch == null || !pouch.CompareTag("Pouch") 
            || pouch.PouchFill == true || !MouseDown)
        {
            return;
        }

        FillPouch(pouch);

        lineRenderer.enabled = true;
    }

    private void FillPouch(Pouch pouch)
    {
        Pouch = pouch;
        Pouch.PouchFill = true;
        Pouch.GetComponent<Collider2D>().enabled = false;
        Pouch.transform.parent = transform;
        pouchRb = pouch.GetComponent<Rigidbody2D>();
        pouchRb.isKinematic = true;
    }

    private void UnfillPouch(Pouch pouch)
    {
        pouchRb.isKinematic = false;
        pouchRb = null;
        Pouch.transform.parent = null;
        Pouch.GetComponent<Collider2D>().enabled = true;
        Pouch.PouchFill = false;
        Pouch = null;
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
            rb.velocity = Direction * Velocity;
            GetComponent<Collider2D>().enabled = false;

            UnfillPouch(Pouch);
            /*pouchRb.isKinematic = false;
            pouchRb = null;
            GetComponent<Collider2D>().enabled = false;
            //rb.velocity = Direction * Velocity; 

            Pouch.transform.parent = null;
            Pouch.GetComponent<Collider2D>().enabled = true;
            Pouch.PouchFill = false;
            Pouch = null;*/

            lineRenderer.enabled = false;

            StartCoroutine("WaitForHit");
        }
    }

    private IEnumerator WaitForHit()
    {
        transform.tag = "Thrown Projectile";
        
        InFlight = true;
        yield return new WaitForSecondsRealtime(FlightTime);
        InFlight = false;

        GetComponent<Collider2D>().enabled = true;


        Debug.Log("can be stuck in");
    }
}
