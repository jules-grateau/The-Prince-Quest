using Assets.Scripts.Controllers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private EventManager eventManager;
    private bool isGamePaused = true;
    private bool isGameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        eventManager = EventManager.current;
        eventManager.onEscapeInput += HandleOnEscapeInput;
        eventManager.onClickButton += HandleClickButton;
        eventManager.onPlayerDie += HandlePlayerDie;
        eventManager.onDoorEnter += HandleDoorEnter;
    }

    void StartGame()
    {
        isGameStarted = true;
        eventManager.OpenScreen(ScreenType.LoadingScreen);
        eventManager.LoadLevel(Level.LevelZero);
        eventManager.ActivateButton(ButtonType.RestartLevel);
        eventManager.OpenScreen(ScreenType.UI);
        eventManager.StartGame();
        ResumeGame();
    }

    void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        eventManager.ResumeGame();
    }

    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        eventManager.ActivateButton(ButtonType.ResumeGame);
        eventManager.PauseGame();
    }

    void RestartLevel()
    {
        eventManager.OpenScreen(ScreenType.LoadingScreen);
        PauseGame();
        eventManager.ReloadLevel();
        ResumeGame();
        eventManager.OpenScreen(ScreenType.UI);
    }

    void HandleOnEscapeInput()
    {
        if(isGameStarted)
        {
            if (isGamePaused)
            {
                ResumeGame();
                eventManager.OpenScreen(ScreenType.UI);
            }
            else
            {
                PauseGame();
                eventManager.OpenScreen(ScreenType.Menu);
            }
        }
    }

    void HandleClickButton(ButtonType buttonType)
    {
        switch(buttonType)
        {
            case ButtonType.StartGame:
                StartGame();
                break;
            case ButtonType.RestartLevel:
                RestartLevel();
                eventManager.OpenScreen(ScreenType.UI);
                break;
            case ButtonType.ResumeGame:
                ResumeGame();
                eventManager.OpenScreen(ScreenType.UI);
                break;
        }
    }

    void HandlePlayerDie()
    {
        eventManager.OpenScreen(ScreenType.DeathScreen);
        eventManager.ActivateButton(ButtonType.RestartLevel);
    }

    void HandleDoorEnter(Level level)
    {
        eventManager.OpenScreen(ScreenType.LoadingScreen);
        eventManager.LoadLevel(level);
        eventManager.OpenScreen(ScreenType.UI);
    }
}
