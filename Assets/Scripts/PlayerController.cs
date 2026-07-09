using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cursorTransform;

    public float maxSpeed = 8f;

    // How tightly the chef snaps to the cursor position. Higher = less glide
    public float followSharpness = 15f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
    }

    void FixedUpdate()
    {
        if (cursorTransform == null) return;

        Vector2 offset = (Vector2)cursorTransform.position - (Vector2)transform.position;
        transform.up = offset.normalized;

        Vector2 desiredVelocity = offset * followSharpness;
        desiredVelocity = Vector2.ClampMagnitude(desiredVelocity, maxSpeed);

        rb.linearVelocity = desiredVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

    }
}