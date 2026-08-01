using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public PlayerController player;
    public BoxCollider2D coll;
    public DropArea[] dropAreas;
    public BoilingPot[] boilingPots;

    public SpriteRenderer sr;

    public float pickUpRange;

    public PlayerController.EquippedIngredients itemType;

    public bool equipped;

    private void Start()
    {
        coll.isTrigger = equipped;
    }

    private void Update()
    {
        if (!equipped)
        {
            if(player.CurrentIngredient != PlayerController.EquippedIngredients.None)
            {
                return; //this fixes a bug where the player was equipping multiple stacked empty bowls and therefore unable to serve soup
            }
            Vector2 distanceToPlayer = player.transform.position - transform.position;
            if (distanceToPlayer.magnitude <= pickUpRange)
            {
                PickUp();
            }
        }
        else
        {
            foreach (DropArea da in dropAreas)
            {
                
                if (da.itemType == itemType && da.areaCollider.OverlapPoint(player.transform.position))
                {
        
                    Drop(da);
                    break;
                }
            }

            foreach (BoilingPot bp in boilingPots)
            {    Debug.Log($"Checking pot overlap: {bp.areaCollider.OverlapPoint(player.transform.position)}, player pos: {player.transform.position}");
                if(bp.areaCollider.OverlapPoint(player.transform.position))
                {
                    DropIntoPot(bp);
                    return;
                }
            }
        }
    }

    private void PickUp()
    {
        equipped = true;
        coll.isTrigger = true;
        sr.enabled = false;
        player.SetEquippedItem(itemType);
    }

    private void Drop(DropArea da)
    {
        equipped = false;
        coll.isTrigger = false;
        sr.enabled = true;
        player.SetEquippedItem(PlayerController.EquippedIngredients.None);
        da.AcceptItem(this);
    }

    private void DropIntoPot(BoilingPot bp)
    {
        equipped = false;
        coll.isTrigger = false;
        sr.enabled = true;
        player.SetEquippedItem(PlayerController.EquippedIngredients.None);
        bp.AcceptItem(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Onion"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}