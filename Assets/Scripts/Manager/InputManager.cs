using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        if (isInputStopped)
            return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            eventManager.EscapeInput();
        }

        if (isGamePaused)
            return;

        eventManager.HorizontalInput(Input.GetAxisRaw("Horizontal"));
        if(Input.GetKeyDown(KeyCode.Space))
        {
            eventManager.SpaceInput();
        }
    }
}
