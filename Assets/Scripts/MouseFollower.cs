using UnityEngine;
using UnityEngine.Serialization;

public class MouseFollower : MonoBehaviour
{
    [FormerlySerializedAs("isFollow")] [FormerlySerializedAs("follow")] [Header("Settings")] 
    public bool isFollowing;
    public float dragSpeed = 20.0f;
    
    private Camera _camera;
    private Rigidbody2D _rb;
    
    private void Awake()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // if (!isFollowing)
        // {
        //     return;
        // }
        
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _rb.transform.position = Vector2.MoveTowards(
            _rb.transform.position,
            new Vector2(mousePos.x, mousePos.y),
            Time.deltaTime * dragSpeed
        );
    }
}