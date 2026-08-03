using System;
using UnityEngine;


public class HoverToExit : MonoBehaviour
{
    public Collider2D buttonCollider;
    public Transform cursorTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        public float hoverTime = 1.5f;

        public StartButton exitButton;


        private bool triggered = false;

        private float hoverTimer;

        
    

    // Update is called once per frame
   void Update()
{
    if (triggered || buttonCollider == null || cursorTransform == null) return;

    bool isHovering = buttonCollider.OverlapPoint(cursorTransform.position);

    if (isHovering)
    {
        hoverTimer += Time.deltaTime;

        if (hoverTimer >= hoverTime)
        {
            triggered = true;
            exitButton.OnExitClick();
        }
    }
    else
    {
        hoverTimer = 0f;
    }
}
}
