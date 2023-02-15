using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MouseFollower))]
[RequireComponent(typeof(Trajectory))]
public class Pouch : MonoBehaviour
{
    [Header("Settings")]
    public float throwSpeed = 10.0f;
    public Vector2 throwPointAnchor = new(0, -2);
    public float throwOffset = 1.5f;

    [Header("Current parameters")] 
    [SerializeField]
    private bool pouchFill;
    public Projectile projectile;
    public float velocity;
    
    private Rigidbody2D _rb;
    private MouseFollower _mouseFollower;
    
    private Vector2 _direction;
    
    private Trajectory _trajectory;
    private SpringJoint2D[] _springJoints2D;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mouseFollower = GetComponent<MouseFollower>();
        _mouseFollower.enabled = false;
        
        _trajectory = GetComponent<Trajectory>();
        _springJoints2D = GetComponents<SpringJoint2D>();
    }

    private void Update()
    {
        if (!pouchFill)
        {
            return;
        }
        
        if (projectile.GetState() == Projectile.State.InPouch)
        {
            _mouseFollower.enabled = true;
            
            // Расчёт угла поворота
            Vector2 currentPosition = transform.position;
            _direction = throwPointAnchor - new Vector2(currentPosition.x, currentPosition.y);
            _direction.Normalize();
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90.0f;
            // И поворот ложи на этот угол
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Расчёт скорости с которой он может полететь
            velocity = Vector2.Distance(currentPosition, throwPointAnchor) * throwSpeed;
        }
        else
        {
            _mouseFollower.enabled = false;
            EmptyPouch();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Projectile"))
        {
            FillPouch(collision.gameObject.GetComponent<Projectile>());
        }
    }

    private void FillPouch(Projectile projectileToFill)
    {
        if (pouchFill || projectileToFill.GetState() != Projectile.State.InPick)
        {
            return;
        }
        
        pouchFill = true;

        GetComponent<Collider2D>().enabled = false;
        
        projectile = projectileToFill;
        projectile.transform.parent = transform;

        projectile.SetState(Projectile.State.InPouch);
        projectile.transform.localPosition = Vector3.zero;

        projectile.GetComponent<MouseFollower>().enabled = false;
        
        //projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;

        _trajectory.Draw();
        
        _springJoints2D[0].enabled = false;
        _springJoints2D[1].enabled = false;
    }

    private void EmptyPouch()
    {
        pouchFill = false;
        
        projectile.SetState(Projectile.State.InCalm);
        if (transform.position.y < throwPointAnchor.y - throwOffset)
        {
            projectile.Shoot(_direction * velocity);
            projectile.SetState(Projectile.State.InFlight);
        }
        
        GetComponent<Collider2D>().enabled = true;
        
        projectile.transform.parent = null;
        projectile = null;
        
        _rb.isKinematic = false;

        _trajectory.NotDraw();
        
        _springJoints2D[0].enabled = true;
        _springJoints2D[1].enabled = true;
    }
}
