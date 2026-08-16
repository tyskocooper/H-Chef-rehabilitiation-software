using System;
using UnityEngine;

public class PrepArea : MonoBehaviour
{
    //PrepArea functions as minigame logic where players collide with hit points to eliminate them

    //h-man cursor/mouse with intracts with hit points
    public Transform cursorTransform;

    //how close the cursor needs to get to the hit point to trigger
    // if patients find the minigames too precise or too easy, adjust this
    public float hitRadius = 0.3f;


    //stops the target being hit more than once
    private bool hit = false;

    //callback which can be used when a target is hit
    public System.Action<PrepArea> onHit;



    void Update()
    {
    //prep area will be an area object that detects the hman cursor entering. 
    // this will allow me to trigger the chopping/stirring miningame.
        if (hit || cursorTransform == null)  return;

        //distance based hit check that checks the position of the h-man cursor relative to the hit radius
        float dist = Vector2.Distance(cursorTransform.position, transform.position);
        if (dist <= hitRadius)
        {
            hit = true;

            //notifys the chopping minigame that this target has been hit
            onHit?.Invoke(this);

            //deactivites the hit target upon collision
            gameObject.SetActive(false);
        }
    }
}
