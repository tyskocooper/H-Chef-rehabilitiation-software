using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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


   // calling chopping minigame
    public ChoppingMinigame choppingMinigame;


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

        if (choppingMinigame != null)
        {
            Vector3 screenCentre = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane + 10f));
            screenCentre.z = 0f;
            choppingMinigame.StartMinigame(screenCentre, () => FinishChopping(item));
        }
        else
        {
            choppingRoutine = StartCoroutine(ChopRoutine(item));
        }
    }

    private void FinishChopping(PickupItem item)
    {
        if (item == null) return;
        
        item.gameObject.SetActive(false);

        if(choppedItem != null)
        {
            SetEquippedItem(PlayerController.EquippedIngredients.None);
            Instantiate(choppedItem, transform.position, Quaternion.identity);
        }
    }

    public void AcceptFromPlayer(PlayerController player)
    {
        if (player.CurrentIngredient != itemType) return;

        SetEquippedItem(itemType);
        player.SetEquippedItem(PlayerController.EquippedIngredients.None);
        ScoreManager.Instance.AddPoints(1);

        if(resetRoutine !=null)
        {
            StopCoroutine(resetRoutine);
        }
        resetRoutine = StartCoroutine(ResetAfterServing());

    }

    private Coroutine resetRoutine;

    private IEnumerator ResetAfterServing()
    {
        yield return new WaitForSeconds(1f); // pausing to give the player enough time to see the finished dish
        SetEquippedItem(PlayerController.EquippedIngredients.None);
        resetRoutine = null;
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


