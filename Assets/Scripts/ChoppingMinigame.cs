using System.Collections.Generic;

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

    //allows me to have one target active a time to encourage the prescribed motion
    private int currentTarget;



    //onComplete lets me call a method to run AFTER the minigame has completed
    private System.Action onComplete;

    private List<PrepArea>activeTargets = new List<PrepArea>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // centrePosition is the where minigame spawns in relation to
  
    public void StartMinigame(Vector3 centrePosition, System.Action onCompleteCallback)
    {
       

        onComplete = onCompleteCallback;
        hitsRemaining = targetOffsets.Count;

       //minigameBackground - activates my dormant background ui (which i've stored as a 2D object in my hierarchy)
       if (minigameBackground != null) minigameBackground.SetActive(true);
       //setInputLocked freezes the chef sprite until the minigame completes
       if (player != null) player.setInputLocked(true);

 
       foreach (Vector2 offset in targetOffsets)
        //This method lets me create spawn points for target object via unity's inpector
        //this is done in relation to the centre of the screen
        {
            Vector3 spawnPosition = centrePosition + (Vector3)offset;
        
            GameObject targetObj = Instantiate(chopTarget, spawnPosition, Quaternion.identity);
            PrepArea target = targetObj.GetComponent<PrepArea>();
           
            //TargetHit is now  a callback I can use in PrepArea when a target is hit
            target.onHit = TargetHit;
            //stores the number of targets I add in the inspector
            activeTargets.Add(target);

        }

        EnableCurrentTarget();
        
        
    }

    private void EnableCurrentTarget()
    {
        Debug.Log($"enabling current target");
        for (int i = 0; i < activeTargets.Count; i++)
        {
             //target.cursor transform informs the target of the cursor position 
            activeTargets[i].cursorTransform = (i == currentTarget) ? cursorTransform: null;
        }
    }

    private void TargetHit(PrepArea zone)
    {
        //reduces the counter of hits remaining
        hitsRemaining--;
        currentTarget ++;

        //if hits remaining equals 0 
        if(hitsRemaining <=0)
        {
            //deactive the minigame background to go back to kitchen scene
            if (minigameBackground != null) minigameBackground.SetActive(false);
            //unlocks the player
            if (player != null) player.setInputLocked(false);
            onComplete?.Invoke();
        
        }
        else
        {
            EnableCurrentTarget();
        }
    }

}
