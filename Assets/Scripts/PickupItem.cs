using UnityEngine;

public class PickupItem : MonoBehaviour
{

    //calls player object which equips the item
    public PlayerController player;

    //collider used for general collission
    public BoxCollider2D coll;

    //drop areas which accept items (chopping board, service area)
    public DropArea[] dropAreas;

    //boiling pot which accepts items
    public BoilingPot[] boilingPots;

    //sprite renders for items/ingredients
    public SpriteRenderer sr;

    //how close the player needs to be to the item to trigger pick up
    public float pickUpRange;


   //assign ingredient type to a pickup item
    public PlayerController.EquippedIngredients itemType;

    //updates to show player is carrying an item
    public bool equipped;

    private void Start()
    {
        //player has nothing equipped by default so collision trigger is active 
        coll.isTrigger = equipped;
    }

    private void Update()
    {
        if (!equipped)
        {   //prevents automatically equipping items if player is already equipped
            //this helped prevent an earlier bug where the play was equipping multiple empty bowls which locked progress
            if(player.CurrentIngredient != PlayerController.EquippedIngredients.None)
            {
                return; //this fixes a bug where the player was equipping multiple stacked empty bowls and therefore unable to serve soup
            }

            //checks position of the player relative to the pick up range
            Vector2 distanceToPlayer = player.transform.position - transform.position;
            if (distanceToPlayer.magnitude <= pickUpRange)
            {
                PickUp();
            }
        }
        else
        {
            //checks if the drop area accepts the specifc item type the player has equipped
            //breaks once a matching area is found
            foreach (DropArea da in dropAreas)
            {
                
                if (da.itemType == itemType && da.areaCollider.OverlapPoint(player.transform.position))
                {
        
                    Drop(da);
                    break;
                }
            }

            //boiling pot needed to be coded seperately so this checks if the boiling pot area accepts the specific item type
            foreach (BoilingPot bp in boilingPots)
            {  
                if(bp.areaCollider.OverlapPoint(player.transform.position))
                {
                    //one item is delivered to the pot, checking for a match stops
                    DropIntoPot(bp);
                    return;
                }
            }
        }
    }

    //function for pick it up an item
    //disables the sprite in the game world and updates the player to show item equipped
    private void PickUp()
    {
        equipped = true;
        coll.isTrigger = true;
        sr.enabled = false;
        player.SetEquippedItem(itemType);
    }


    //drops the item into the DropArea(service area, chopping board)
    private void Drop(DropArea da)
    {
        equipped = false;
        coll.isTrigger = false;
        sr.enabled = true;
        player.SetEquippedItem(PlayerController.EquippedIngredients.None);
        da.AcceptItem(this);
    }


  //drops the item into the boiling pot
    private void DropIntoPot(BoilingPot bp)
    {
        equipped = false;
        coll.isTrigger = false;
        sr.enabled = true;
        player.SetEquippedItem(PlayerController.EquippedIngredients.None);
        bp.AcceptItem(this);
    }

    //deactives the matching ingredient when collider is triggered (in this case an unchopped onion)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Onion"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}