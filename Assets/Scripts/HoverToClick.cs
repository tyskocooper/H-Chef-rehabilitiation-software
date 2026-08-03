using System;
using JetBrains.Annotations;
using UnityEngine;


public class HoverToClick : MonoBehaviour
{
    public Collider2D buttonCollider;
    public Transform cursorTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        public float hoverTime = 1.5f;

        public StartButton startButton;
        public ServiceOver serviceOver;

        public enum ClickAction { Start, Exit, Restart, ExitToMenu}
        public ClickAction action = ClickAction.Start;


        private bool triggered = false;

        private float hoverTimer;

        
    

    // Update is called once per frame
   void Update()
{
    if (triggered || buttonCollider == null || cursorTransform == null) return;

    bool isHovering = buttonCollider.OverlapPoint(cursorTransform.position);

    if (isHovering)
    {
        //changed from deltaTime to unscaled deltatime
        //this is because delta time does not count during ServiceOver as deltatime is set to 0f
        hoverTimer += Time.unscaledDeltaTime;
        

        if (hoverTimer >= hoverTime)
        {
            triggered = true;
          

            if (action == ClickAction.Start)
            startButton.onStartClick();

            if(action == ClickAction.Exit)
            startButton.OnExitClick();

            if (action == ClickAction.Restart)
            serviceOver.OnRestartPressed();

            if (action == ClickAction.ExitToMenu)
            serviceOver.OnExitPressed();
            
        }
    }
    else
    {
        hoverTimer = 0f;
    }
}
}
