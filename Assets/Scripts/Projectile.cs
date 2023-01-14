using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Destroyable))]
public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    public string projectileName = "None";
    [Space]
    public float flightTime = 1.0f;
    public int damage = 1;

    [Header("Special settings")]
    public float dragSpeed = 20.0f;
    public float throwOffset = 1.5f; 
    public float finalScale = 0.3f;

    [Header("Current parameters")]
    public float velocity;
    public Pouch pouch;
    public Vector2 startPosition;

    public State state;
    
    public event Action OnFlight;
    
    private Vector2 _direction;
    private float _scaleVelocity;

    private Camera _camera;
    private bool _mouseDown;
    private Rigidbody2D _rb;

    private LineRenderer _lineRenderer;

    public enum State
    {
        InCalm,
        InPouch,
        InFlight
    }
    
    private void Awake()
    {
        _camera = Camera.main;
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
            Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            _rb.transform.position = Vector2.MoveTowards(
                _rb.transform.position,
                new Vector2(mousePos.x, mousePos.y),
                Time.deltaTime * dragSpeed
                );

            // Пока снаряд не вылетел из рогатки
            if (state == State.InPouch)
            {
                // Расчёт угла поворота
                Vector2 currentPosition = transform.position;
                _direction = startPosition - new Vector2(currentPosition.x, currentPosition.y);
                _direction.Normalize();
                float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90.0f;
                // И поворот рогатки на этот угол
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Расчёт скорости с которой он может полететь
                velocity = Vector2.Distance(currentPosition, startPosition) * pouch.throwSpeed;

                // Установка нормального положения снаряда в рогатке
                var pouchTransform = pouch.transform;
                pouchTransform.localPosition = new Vector2(0, -0.5f);
                pouchTransform.localRotation = Quaternion.identity;
            }
        }
    }

    private void FixedUpdate()
    {
        // Уменьшение снаряда во время полёта
        if (state == State.InFlight)
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
        if (collision == null || !collision.gameObject.CompareTag("Pouch"))
        {
            return;
        }

        Pouch collisionPouch = collision.gameObject.GetComponent<Pouch>();
        if (collisionPouch == null
            || collisionPouch.pouchFill 
            || !_mouseDown)
        {
            return;
        }

        FillPouch(collisionPouch);

        _lineRenderer.enabled = true;
    }

    private void FillPouch(Pouch pouchToFill)
    {
        pouch = pouchToFill;
        pouch.pouchFill = true;
        pouch.GetComponent<Collider2D>().enabled = false;
        pouch.transform.parent = transform;
        pouch.GetComponent<Rigidbody2D>().isKinematic = true;

        state = State.InPouch;
        startPosition = pouch.startPosition;
    }

    private void EmptyPouch()
    {
        pouch.GetComponent<Rigidbody2D>().isKinematic = false;
        pouch.transform.parent = null;
        pouch.GetComponent<Collider2D>().enabled = true;
        pouch.pouchFill = false;
        pouch = null;

        state = State.InCalm;
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

        if (pouch != null)
        {
            EmptyPouch();
            _lineRenderer.enabled = false;

            Debug.Log($"{transform.position.y} / {startPosition.y - throwOffset}({startPosition.y} - {throwOffset})");
            if (transform.position.y < startPosition.y - throwOffset)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        gameObject.layer = LayerMask.NameToLayer("Middleground");
        _rb.velocity = _direction * velocity;
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(nameof(WaitForHit));
    }

    private IEnumerator WaitForHit()
    {
        state = State.InFlight;
        OnFlight?.Invoke();
        yield return new WaitForSecondsRealtime(flightTime);
        state = State.InCalm;

        GetComponent<Collider2D>().enabled = true;
        Debug.Log($"{projectileName} can be stuck in target");
        
        yield return new WaitForSecondsRealtime(0.1f);

        gameObject.layer = LayerMask.NameToLayer("Background");
    }
}
