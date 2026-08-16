using System.Collections;
using UnityEngine;

public class BoilingPot : MonoBehaviour
{
    //collider used for general collission
    public BoxCollider2D coll;

    //collider that triggers when the player's collision zone overlaps the pot's
    public BoxCollider2D areaCollider;

    //used to show the pots various states - empty, boiling and ready
    public SpriteRenderer sr;

    //calling player controller the correct 'equipped' sprite can be set in the inspector upon collision 
    public PlayerController player;

    public bool equipped;

    //boling pot state - empty/boing/ready 
    private enum PotState { Empty, Boiling, PotReady }
    private PotState state = PotState.Empty;
    
    //allows sprites to be set in the inspector which updates the pot's appearance depending on the current state
    public Sprite emptyPotSprite;
    public Sprite BoilingSprite;
    public Sprite PotReadySprite;

    //how long the onion needs to boil before it is ready, this helped fix a bug where the player equipped the onion to early, cancelling the minigame
    public float onionBoilTime = 3f;

    //this couritine allows the boiling pot to have an ongoing setting so that it cannot be triggered during the wrong state
    private Coroutine boilingRoutine;


    // calling chopping minigame and repurposing it as stirring
    public ChoppingMinigame stirringMinigame;

    //prevents the stir minigame from being triggered again while boiling is ongoing
    private bool isStirring = false;


    // checks if the stirring minigame has been completed before finishing boiling state
    private bool hasStirred = false;


    void Start()
    {
       

        if (areaCollider == null)
        {
            areaCollider = GetComponent<BoxCollider2D>();
        }
        areaCollider.isTrigger = true;

        UpdateSprite();
    }

    //called when the player drops/uses an item on the pot
    public void AcceptItem(PickupItem item)
    {
    // first stage - empty pot - accepts chopped onion
        if (state == PotState.Empty && item.itemType == PlayerController.EquippedIngredients.choppedOnion)
        {
            if (boilingRoutine != null) return;

    //resets stirring minigame so all hit points are reactivated 
            hasStirred = false;
            boilingRoutine = StartCoroutine(OnionBoilRoutine(item));
            return;
        }

    // second stage - pot is ready and boiled - soup is ready to dish into empty bowl
        if (state == PotState.PotReady && item.itemType == PlayerController.EquippedIngredients.emptyBowl)
        {
            FillBowl(item);
            return;
        }

    // all other ingredients are ignored
    }

    // handles the boiling process: switch to boiling state, wait for timer, wait until stirring has completed
    private IEnumerator OnionBoilRoutine(PickupItem item)
    {
        state = PotState.Boiling;
        UpdateSprite();
        
        //onion object is destoyed once boiling is activated. this prevents the player accidentally equipping it later
        Destroy(item.gameObject);

        yield return new WaitForSeconds(onionBoilTime);
        yield return new WaitUntil(() => hasStirred);

        state = PotState.PotReady;
        UpdateSprite();


    //clears the routine so boiling can be retriggered later
        boilingRoutine = null;
    }

    private void FillBowl(PickupItem item)
    {
        PlayerController itemPlayer = item.player;

        Destroy(item.gameObject);

        if (itemPlayer != null)
        {
            itemPlayer.SetEquippedItem(PlayerController.EquippedIngredients.FilledBowl);
        }

        //pot resets after dishing out, ready for another onion
        state = PotState.Empty;
        UpdateSprite();
    }

    // if player collides with the boiling pot, this will begin an attempt to start the stirring minigame
    public void TryStir()
    {
        //can only stir while boiling, once per boil minigame instance, and not while stirring is ongoing
        if (state != PotState.Boiling || hasStirred || isStirring || stirringMinigame == null) return;

        isStirring = true;
        
        // triggers the stirring minigame to trigger in a centred position
        Vector3 screenCentre = Camera.main.transform.position;
        screenCentre.z = 0f;
        stirringMinigame.StartMinigame(screenCentre, FinishStirring);
    }

    //runs once stirring minigame has been completed
    private void FinishStirring()
    {
      hasStirred = true;
      isStirring = false;
    }

    //updates the pots sprite to match the correct state
    private void UpdateSprite()
    {
        if (sr == null) return;

        sr.sprite = state switch
        {
            PotState.Boiling => BoilingSprite,
            PotState.PotReady => PotReadySprite,
            _ => emptyPotSprite
        };
    }
}