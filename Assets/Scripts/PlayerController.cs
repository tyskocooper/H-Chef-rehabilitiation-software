using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cursorTransform;

    public float maxSpeed = 8f;

    // How tightly the chef snaps to the cursor position. Higher = less glide
    public float followSharpness = 15f;

    public enum EquippedIngredients
    {
        None,
        Onion,

        Carrot,

        choppedOnion,

        emptyBowl,

        FilledBowl,
    }

    public DropArea[] serviceAreas;
    
    public EquippedIngredients CurrentIngredient {get; private set;} = EquippedIngredients.None;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
    }

    void Update()
    {
        if (cursorTransform == null) return;

        Vector2 offset = (Vector2)cursorTransform.position - (Vector2)transform.position;
        transform.up = offset.normalized;

        Vector2 desiredVelocity = offset * followSharpness;
        desiredVelocity = Vector2.ClampMagnitude(desiredVelocity, maxSpeed);

        rb.linearVelocity = desiredVelocity;

        foreach (DropArea area in serviceAreas)
        {
            if (area.areaCollider.OverlapPoint(transform.position))
            {
                if(area.areaCollider.OverlapPoint(transform.position))
                {
                    area.AcceptFromPlayer(this);
                    break;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


    }

    //ingredient pick ups

    public SpriteRenderer chefRenderer;
    public Sprite normalSprite;
    public Sprite onionSprite;

    public Sprite choppedOnionSprite;

    public Sprite emptyBowl;

    public Sprite filledBowl;

    public void SetEquippedItem(EquippedIngredients item)
    {
        CurrentIngredient = item;
        chefRenderer.sprite = item switch
        {
        EquippedIngredients.Onion => onionSprite,
        EquippedIngredients.choppedOnion => choppedOnionSprite,
        EquippedIngredients.emptyBowl => emptyBowl,
        EquippedIngredients.FilledBowl => filledBowl, 
        _=> normalSprite

        };
    }

}