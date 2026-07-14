using UnityEngine;

public class DropArea : MonoBehaviour
{

    public BoxCollider2D areaCollider;

    
    public BoxCollider2D coll;

    public SpriteRenderer sr;

    public PlayerController player;

    public PlayerController.EquippedIngredients itemType;


    public bool equipped;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coll.isTrigger = equipped;

        if (areaCollider == null)
        {areaCollider = GetComponent<BoxCollider2D>();
        }
        areaCollider.isTrigger = true;
    }


    //ingredient pick ups

    public SpriteRenderer dropOffRenderer;
    public Sprite normalSprite;
    public Sprite onionSprite;

    public void SetEquippedItem(PlayerController.EquippedIngredients item)
    {
        Debug.Log("DropArea SetEquippedItem called with: " + item);
        
        dropOffRenderer.sprite = item switch
        {
        PlayerController.EquippedIngredients.Onion => onionSprite,

        _ => normalSprite
        };
    }

}


