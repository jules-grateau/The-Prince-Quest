using System;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
 /*   private EventManager eventManager;
    private bool isGameStarted = false;
    void Start()
    {
        eventManager = EventManager.current;
        ManageScreenOpening(ScreenType.Menu);
        eventManager.onResumeGame += HandleResumeGame;
        eventManager.onPauseGame += HandlePauseGame;
        eventManager.onStartGame += HandleOnStartGame;
        eventManager.onPlayerDie += HandlePlayerDie;
    }

    void HandleOnStartGame()
    {
        isGameStarted = true;
    }
    void HandleResumeGame()
    {
        ManageScreenOpening(ScreenType.UI);
    }

    void HandlePauseGame()
    {
        ManageScreenOpening(ScreenType.Menu);
    }

    void HandlePlayerDie()
    {
        ManageScreenOpening(ScreenType.DeathScreen);
    }

    void ManageScreenOpening(ScreenType screenTypeToOpen)
    {
        foreach(ScreenType screenType in Enum.GetValues(typeof(ScreenType)))
        {
            if(screenTypeToOpen == screenType)
            {
                eventManager.OpenScreen(screenType);
            } else
            {
                eventManager.CloseScreen(screenType);
            }
        }
    }*/
}
