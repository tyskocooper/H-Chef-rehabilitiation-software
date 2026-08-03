using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cursorTransform;

    public float maxSpeed = 4f;

    // How tightly the chef snaps to the cursor position. Higher = less glide
    public float followSharpness = 6f;

    //options to lock the chef sprite when a minigame is active
    public bool inputLocked = false;


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
    public BoilingPot[] boilingPots;
    
    public EquippedIngredients CurrentIngredient {get; private set;} = EquippedIngredients.None;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
    }

    // input locked sets velocity to zero so that the chef doesnt follow the cursor
    public void setInputLocked(bool locked)
    {
        inputLocked = locked;
        if (locked) rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (inputLocked || cursorTransform == null) return;

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

          foreach (BoilingPot bp in boilingPots)
        {   

            if (bp.areaCollider.OverlapPoint(transform.position))
            {
                bp.TryStir();
                    break;
                
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
