using System;
using JetBrains.Annotations;
using UnityEngine;


public class HoverToClick : MonoBehaviour
{

    //collider for the button, cursor must overlap to trigger
    public Collider2D buttonCollider;

    //H-Man Cursor/mouse cursor's transform, checks hover posotion
    public Transform cursorTransform;

    //how long the cursor must hover before triggering the 'click' action
        public float hoverTime = 1.5f;

        //references the start button menu and service over menu
        public StartButton startButton;
        public ServiceOver serviceOver;


        //stores actions for specific button triggers
        public enum ClickAction { Start, Exit, Restart, ExitToMenu}
        public ClickAction action = ClickAction.Start;

        //prevents the action firing more than once while hovering
        private bool triggered = false;

        //tracks how long the cursor has been hovering for
        private float hoverTimer;

        
    
   void Update()
    {
        //skip if already triggers, stops repeat triggering causing freeze
    if (triggered || buttonCollider == null || cursorTransform == null) return;


    //checks the cursor position relative to the button. true on overlap
    bool isHovering = buttonCollider.OverlapPoint(cursorTransform.position);

    if (isHovering)
    {
        //had to change from deltaTime to unscaled deltatime
        //this is because delta time does not count during ServiceOver screen as deltatime is set to 0f
        hoverTimer += Time.unscaledDeltaTime;
        
        //after hover time is greater or exual to the hover timer, executes the action
        if (hoverTimer >= hoverTime)
        {
            triggered = true;
          
            //pairs click actions to matching buttons
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
        //hover timer resets when cursor is not overlapping
        hoverTimer = 0f;
    }
}
}
