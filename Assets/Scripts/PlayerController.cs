using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 1f;

    [Tooltip("H-MAN displacement (metres) that maps to full direction magnitude. Calibrate against live device.")]
    public float hmanRange = 0.1f;

    [Tooltip("Minimum displacement before H-MAN input counts as intentional (deadzone, in metres).")]
    public float hmanDeadzone = 0.005f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(
        
    )
    {
        bool useHman = HManConnection.Instance != null && HManConnection.Instance.IsConnected;
        if (useHman)
        Debug.Log($"HMAN raw: {HManConnection.Instance.LocationX}, {HManConnection.Instance.LocationY}");

        Vector2? direction = useHman ? GetHmanDirection() : GetMouseDirection();

        if (direction.HasValue)
        {
            transform.up = direction.Value;
            rb.AddForce(direction.Value * thrustForce);
        }
    }

    private Vector2? GetHmanDirection()
    {
        float x = HManConnection.Instance.LocationX;
        float y = HManConnection.Instance.LocationY;
        Vector2 raw = new Vector2(x, y);

        if (raw.magnitude < hmanDeadzone) return null; // ignore tiny drift near origin

        return (raw / hmanRange).normalized;
    }

    private Vector2? GetMouseDirection()
    {
        if (!Mouse.current.leftButton.isPressed) return null;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        return ((Vector2)(mousePos - transform.position)).normalized;
    }

    

    //void OnCollisionEnter2D(Collision2D collision)
    //{
       ///= Destroy(gameObject);
    //}
}