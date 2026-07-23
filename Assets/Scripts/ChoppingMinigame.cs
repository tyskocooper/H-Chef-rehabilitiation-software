using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class ChoppingMinigame : MonoBehaviour
{

    //prefab for a single chop point. the player will need to collide their hman cursor with this point to disable it
    public GameObject chopTarget;
    public Transform cursorTransform;

    //where targets spawn in relation to the centre of the scren
    public List<Vector2>targetOffsets;

    //adds a miningame background so the kitchen is obscured during chopping. makes the ui less messy 
    public GameObject minigameBackground;

    //calling PlayerController so I can pause the chef sprite during minigames.
    //this way, when the minigame ends, the chef is still in the same position
    public PlayerController player;


    
    // hit tracker that decreases upon each target hit by the cursor
    private int hitsRemaining;
    private System.Action onComplete;

    private List<PrepArea>activeTargets = new List<PrepArea>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartMinigame(Vector3 centrePosition, System.Action onCompleteCallback)
    {
       

        onComplete = onCompleteCallback;
        hitsRemaining = targetOffsets.Count;
        activeTargets.Clear();

       if (minigameBackground != null) minigameBackground.SetActive(true);
       if (player != null) player.setInputLocked(true);

       foreach (Vector2 offset in targetOffsets)

        {
            Vector3 spawnPosition = centrePosition + (Vector3)offset;
        
            GameObject targetObj = Instantiate(chopTarget, spawnPosition, Quaternion.identity);
            PrepArea target = targetObj.GetComponent<PrepArea>();
            target.cursorTransform = cursorTransform;
            target.onHit = TargetHit;
            activeTargets.Add(target);

        }
        
        
    }

    private void TargetHit(PrepArea zone)
    {
        hitsRemaining--;
        if(hitsRemaining <=0)
        {
            if (minigameBackground != null) minigameBackground.SetActive(false);
            if (player != null) player.setInputLocked(false);
            onComplete?.Invoke();
        
        }
    }

}
