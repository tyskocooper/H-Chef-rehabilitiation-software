using System;
using UnityEngine;

public class PrepArea : MonoBehaviour
{
    public Transform cursorTransform;


    public float hitRadius = 0.3f;

    private bool hit = false;
    public System.Action<PrepArea> onHit;


    // Update is called once per frame
    void Update()
    {
    //prep area will be a collider object that detects the hman cursor entering. 
    // this will allow me to trigger the chopping/stirring miningame.
        if (hit || cursorTransform == null)  return;
        float dist = Vector2.Distance(cursorTransform.position, transform.position);
        if (dist <= hitRadius)
        {
            hit = true;
            onHit?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
