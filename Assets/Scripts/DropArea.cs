using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{

    public BoxCollider2D areaCollider;

    
    public BoxCollider2D coll;

    public SpriteRenderer sr;

    public PlayerController player;

    public PlayerController.EquippedIngredients itemType;


    public bool equipped;

    [System.Serializable]
    public class ItemSprite
    {
        public PlayerController.EquippedIngredients itemType;
        public Sprite sprite;
    }

    public List<ItemSprite> itemSprites;
    public Sprite emptySprite;

    public GameObject choppedItem;
    public float chopTime = 2f;


    //Coroutine is a function that can suspend its execution (yield) unitl the YieldInstruction finishes
    private Coroutine choppingRoutine;

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
        
        Sprite spriteToShow = emptySprite;

        foreach (var entry in itemSprites)
        {
            if (entry.itemType == item)
            {
                spriteToShow = entry.sprite;
                break;
            }
        }

        dropOffRenderer.sprite = spriteToShow;
    }
    

    public void AcceptItem(PickupItem item)
    {
        SetEquippedItem(item.itemType);
        
        if (choppingRoutine != null)
        {
            StopCoroutine(choppingRoutine);
        }
        choppingRoutine = StartCoroutine(ChopRoutine(item));

    }

    //IEnumerator is the return tyoe for a coroutine. 
    private IEnumerator ChopRoutine(PickupItem item)
    {
        yield return new WaitForSeconds(chopTime);

    
        Destroy(item.gameObject);

        //Insantiate creates a copy of the choppedItem gameObject(like dragging a prefab into the hierarchry midgame)
        //added if statment so to prevent unprompted copies
        
        if (choppedItem != null)
        {
            SetEquippedItem(PlayerController.EquippedIngredients.None);
            Instantiate(choppedItem, transform.position, Quaternion.identity);
        }
        choppingRoutine = null;
    }
}


