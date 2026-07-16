using UnityEngine;

public class BoilingPot : MonoBehaviour
{

    public enum EquippedIngredients
    {
        None,
        Onion
    }

    public BoxCollider2D coll;

    public SpriteRenderer sr;

    public PlayerController player;

    public PlayerController.EquippedIngredients itemType;


    public bool equipped;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         coll.isTrigger = equipped;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
