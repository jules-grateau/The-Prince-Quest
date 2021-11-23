using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float spaceKeyForcePerFrameFixed = 0.1f;
    public float maxSpaceKeyForce = 1f;

    private EventManager eventManager;
    private bool isGamePaused = true;
    private bool isInputStopped = false;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onResumeGame += HandleResumeGame;
        eventManager.onPauseGame += HandlePauseGame;
        eventManager.onStopPlayerInput += HandleInputStop;
    } 

    void HandleInputStop(bool shouldInputStop)
    {
        isInputStopped = shouldInputStop;
    }

    void HandleResumeGame()
    {
        isGamePaused = false;
    }

    void HandlePauseGame()
    {
        isGamePaused = true;
    }


    private void Update()
    {
        if (isInputStopped)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            eventManager.EscapeInput();
        }

        if (isGamePaused)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventManager.SpaceInputDown();
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            eventManager.SpaceInputUp();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            eventManager.InteractKeyDown();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            eventManager.InteractKeyUp();
        }
    }

    void FixedUpdate()
    {
        if (isInputStopped)
            return;

        if (isGamePaused)
            return;

        eventManager.HorizontalInput(Input.GetAxisRaw("Horizontal"));

        if(Input.GetKey(KeyCode.Space))
        {
            eventManager.SpaceInput();
        }
    }
}
