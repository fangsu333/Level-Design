using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameStateController : MonoBehaviour
{

    //some variables and behaviours were moved from the Player to this class to make
    //the game more organised
    
    //duration of the level in seconds
    public static float levelDuration;

    //controls whether the game is running (true) or paused/over (false)
    public static bool gameRunning;

    //points that the player has
    public static int score;

    //lives that the player has
    public static int lives;

    //reference to the DialogueSystem (another component in the same GameObject)
    private DialogueSystem dialogueSys;
    //reference to the UIController (another component in the same GameObject)
    private UIController ui;

    //true if the game is stopped at an event (e.g. cutscene), false if not
    private static bool isInEvent;

    //holds the current (or the last event) that happened in game
    private DialogueLine currentEvent;

    //reference to allObstacles - this is used to make the player temporarily invincible after
    //hitting an obstacle, so the game does not stop/break
    public GameObject allObstacles;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        lives = 5;

        levelDuration = 99999;

        dialogueSys = GetComponent<DialogueSystem>();
        ui = GetComponent<UIController>();

        isInEvent = false;

        gameRunning = true;

        //updates the ui using the variable lives above (Check UIController if needed)
        ui.RefreshLives();

        //updates the score using the variable score above (Check UIController if needed)
        ui.RefreshScore();
    }

    // Update is called once per frame
    void Update()
    {
        //you already know this first part of the code
        //it was in the PlayerBehaviour

        //checks the conditions to make sure the game keeps running and stops
        //if the player died or time ran out
        if (gameRunning)
        {
            levelDuration -= Time.deltaTime;

            if ((lives <= 0) || (levelDuration <= 0))
            {
                GameOver();
            }
        }
        //this is a new part that controls events ("cutscenes")
        //we just make sure that if we are stopped at a cutscene and the player clicks
        //the mouse, we move to the next event (or finish, if it is the final event in a sequence...)
        else
        {
            if (isInEvent)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    MoveToNextEvent();
                }
            }
        }
    }

    //stops gameplay and asks the UI component to display the game over message
    void GameOver()
    {
        Pause();
        ui.DisplayGameOver();
    }

    //pauses gameplay using Time.timeScale (all Physics running at FixedUpdate are stopped)
    void Pause()
    {
        gameRunning = false;
        Time.timeScale = 0f;
    }

    //restores gameplay
    void Unpause()
    {
        gameRunning = true;
        Time.timeScale = 1f;
        
    }

    //This is usually called by a TriggerEvent
    //Finds the right event and displays the static cutscene
    public void StartEvent(int eventID)
    {
        //pauses gameplay
        Pause();

        //this bool is used to control whether update should read and use clicks (see Update function above)
        isInEvent = true;

        //Retrieves the correct "narrative event" (as a DialogueLine) using the eventid received from whoever called this function
        //(usually the trigger)
        currentEvent = dialogueSys.GetEvent(eventID);      

        //passes the DialogueLine object to the ui so it can display the content (text, image, audio)
        ui.DisplayEvent(currentEvent);        

    }

    //THis is usually called from Update
    //when there is a click and "isInEvent" is true
    private void MoveToNextEvent()
    {        
        //if this is the last event in the sequence we need to get back to gameplay
        if (currentEvent.lastEventInSequence)
        {
            //the game will be unpaused
            Unpause();
            //the dialogue box will be hidden
            ui.HideDialogue();
            //and the bool isInEvent will be set to false (as we are back to gameplay)
            isInEvent = false;
        }
        //otherwise, we start the next "narrative event" in that sequence
        else
        {
            //see function above
            StartEvent(currentEvent.nextEvent);
        }
    }




    public void UpdateScore(int points)
    {
        score += points;
        ui.RefreshScore();
    }


    public void UpdateLives(int difference)
    {
        lives += difference;
        ui.RefreshLives();
    }


    public void MakeInvincible()
    {
        foreach (Collider c in allObstacles.GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
    }

    public void MakeVulnerable()
    {
        foreach (Collider c in allObstacles.GetComponentsInChildren<Collider>())
        {
            c.enabled = true;
        }
    }
}
