using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    public string projectileName = "None";
    public float dragSpeed = 20.0f;
    public float flightTime = 1.0f;
    public float finalScale = 0.3f;
    public int damage = 1;

    [Header("Current parameters")]
    public float velocity;
    public Pouch pouch;
    public Vector2 startPosition;

    private Vector2 _direction;
    private float _scaleVelocity;
    private bool _inPouch = false;
    private bool _inFlight = false;

    private bool _mouseDown = false;
    private Rigidbody2D _rb;
    private Rigidbody2D _pouchRb;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _rb.isKinematic = false;

        _scaleVelocity = (1 - finalScale) / flightTime;
    }

    private void Update()
    {
        if (_mouseDown)
        {
            // Плавно следовать за мышью
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _rb.transform.position = Vector2.MoveTowards(
                _rb.transform.position,
                new Vector2(mousePos.x, mousePos.y),
                Time.deltaTime * dragSpeed
            );

            // Пока снаряд не вылетел из рогатки
            if (_inPouch)
            {
                // Расчёт угла поворота
                _direction = startPosition - new Vector2(transform.position.x, transform.position.y);
                _direction.Normalize();
                var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90.0f;
                // И поворот рогатки на этот угол
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Расчёт скорости с которой он может полететь
                velocity = Vector2.Distance(transform.position, startPosition) * pouch.throwSpeed;

                // Установка нормального положения снаряда в рогатке
                pouch.transform.localPosition = new Vector2(0, -0.5f);
                pouch.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void FixedUpdate()
    {
        // Уменьшение снаряда во время полёта
        if (_inFlight)
        {
            Vector3 newScale = _rb.transform.localScale - new Vector3(
                _scaleVelocity * Time.deltaTime,
                _scaleVelocity * Time.deltaTime, 
                0);
            if (newScale.x > 0 || newScale.y > 0)
            {
                _rb.transform.localScale = newScale;
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
            || pouch.pouchFill || !_mouseDown)
        {
            return;
        }

        FillPouch(pouch);

        _lineRenderer.enabled = true;
    }

    private void FillPouch(Pouch pouch)
    {
        this.pouch = pouch;
        this.pouch.pouchFill = true;
        this.pouch.GetComponent<Collider2D>().enabled = false;
        this.pouch.transform.parent = transform;
        _pouchRb = pouch.GetComponent<Rigidbody2D>();
        _pouchRb.isKinematic = true;

        _inPouch = true;
        startPosition = this.pouch.startPosition;
    }

    private void EmptyPouch()
    {
        _pouchRb.isKinematic = false;
        _pouchRb = null;
        pouch.transform.parent = null;
        pouch.GetComponent<Collider2D>().enabled = true;
        pouch.pouchFill = false;
        pouch = null;

        _inPouch = false;
        startPosition = Vector2.zero;
    }

    private void OnMouseDown()
    {
        _mouseDown = true;

        _rb.isKinematic = true;

        _rb.velocity = Vector2.zero;
        velocity = 0;
        _rb.angularVelocity = 0;
    }

    private void OnMouseUp()
    {
        _mouseDown = false;

        _rb.isKinematic = false;


        if (_pouchRb != null)
        {
            _rb.velocity = _direction * velocity;
            GetComponent<Collider2D>().enabled = false;

            EmptyPouch();

            _lineRenderer.enabled = false;

            StartCoroutine("WaitForHit");
        }
    }

    private IEnumerator WaitForHit()
    {
        transform.tag = "Thrown Projectile";
        
        _inFlight = true;
        yield return new WaitForSecondsRealtime(flightTime);
        _inFlight = false;

        GetComponent<Collider2D>().enabled = true;

        Debug.Log(projectileName + " can be stuck in target");
    }
}
