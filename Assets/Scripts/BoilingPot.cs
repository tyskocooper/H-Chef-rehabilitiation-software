using System.Collections;
using UnityEngine;

public class BoilingPot : MonoBehaviour
{
    public BoxCollider2D coll;

    public BoxCollider2D areaCollider;

    public SpriteRenderer sr;

    public PlayerController player;

    public bool equipped;

    //boling pot state - empty/boing/ready
    private enum PotState { Empty, Boiling, PotReady }
    private PotState state = PotState.Empty;


    public Sprite emptyPotSprite;
    public Sprite BoilingSprite;
    public Sprite PotReadySprite;
    public float onionBoilTime = 3f;

    private Coroutine boilingRoutine;

    void Start()
    {
       

        if (areaCollider == null)
        {
            areaCollider = GetComponent<BoxCollider2D>();
        }
        areaCollider.isTrigger = true;

        UpdateSprite();
    }

    public void AcceptItem(PickupItem item)
    {
        // first stage - empty pot - accepts chopped onion
        if (state == PotState.Empty && item.itemType == PlayerController.EquippedIngredients.choppedOnion)
        {
            if (boilingRoutine != null) return;
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

    private IEnumerator OnionBoilRoutine(PickupItem item)
    {
        state = PotState.Boiling;
        UpdateSprite();

        Destroy(item.gameObject);

        yield return new WaitForSeconds(onionBoilTime);

        state = PotState.PotReady;
        UpdateSprite();

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