using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public PlayerController player;
    public BoxCollider2D coll;
    public DropArea da;

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
            Vector2 distanceToPlayer = player.transform.position - transform.position;
            if (distanceToPlayer.magnitude <= pickUpRange)
            {
                PickUp();
            }
        }
        else
        {
            if (da.areaCollider.OverlapPoint(player.transform.position))
            {
                Drop();
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

    private void Drop()
    {
        equipped = false;
        coll.isTrigger = false;
        sr.enabled = true;
        player.SetEquippedItem(PlayerController.EquippedIngredients.None);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Onion"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}