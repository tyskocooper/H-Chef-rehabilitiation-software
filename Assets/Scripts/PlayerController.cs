using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //the H-man cursor/mouse the player(chef) follows
    public Transform cursorTransform;
    
    //speed the chef can move at
    //slow or speed this up depending on preference
    public float maxSpeed = 4f;

    // How tightly the player snaps to the cursor position. Higher = less glide
    public float followSharpness = 6f;

    //options to lock the player sprite when a minigame is active
    public bool inputLocked = false;


    //all possible items the player can equip
    public enum EquippedIngredients
    {
        None,
        Onion,

        Carrot,

        choppedOnion,

        emptyBowl,

        FilledBowl,
    }


    //drop area for finished dishes
    public DropArea[] serviceAreas;

    //drop area for the boiling pot
    public BoilingPot[] boilingPots;
    
    //changes what the player currently has equipped
    public EquippedIngredients CurrentIngredient {get; private set;} = EquippedIngredients.None;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; //gravity is in effect by default in unity editor, 0f disables this
        rb.linearDamping = 0f; //movement is controlled via velocity with no dragging
    }

    // this function sets velocity to zero so that the player doesnt follow the cursor
    //suitable duiring minigames when the chef sprite should be paused
    public void setInputLocked(bool locked)
    {
        inputLocked = locked;
        if (locked) rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (inputLocked || cursorTransform == null) return;

        //distance+direction between the chef sprite and the cursor
        Vector2 offset = (Vector2)cursorTransform.position - (Vector2)transform.position;

        //chef sprite rotates to the direction of movement
        transform.up = offset.normalized;

        //the velocity increases the further away the cursor is from the player
        //helps the player guide the chef sprite to far away areas faster
        Vector2 desiredVelocity = offset * followSharpness;
        desiredVelocity = Vector2.ClampMagnitude(desiredVelocity, maxSpeed);

        rb.linearVelocity = desiredVelocity;


        //checks if the chefsprite overlaps the service area colllider to deliver the finished dish
        foreach (DropArea area in serviceAreas)
        {
            if (area.areaCollider.OverlapPoint(transform.position))
            {
                if(area.areaCollider.OverlapPoint(transform.position))
                {
                    area.AcceptFromPlayer(this);
                    //stops checking once service area is triggered successfully
                    break;
                }
              }
        }
        //checks if the chefsprite overlaps the boiling pot colllider to trigger the stirring minigame
  
          foreach (BoilingPot bp in boilingPots)
        {   

            if (bp.areaCollider.OverlapPoint(transform.position))
            {
                bp.TryStir();
                    break; //stops checking once the boiling pot is triggered successfully
                
              }
        }
    }

    //ingredient pick ups
    public SpriteRenderer chefRenderer;
    public Sprite normalSprite;
    public Sprite onionSprite;

    public Sprite choppedOnionSprite;

    public Sprite emptyBowl;

    public Sprite filledBowl;

    //updates what the player is holding. 
    // CurrentIngredient for logic when interacting with drop areas
    // Sprites for visual feedback
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
