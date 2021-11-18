using Assets.Scripts.Controllers.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private EventManager eventManager;
    private bool isGamePaused = true;
    private bool isGameStarted = false;
    private int playerLifes = 0;
    private const string lifeBoxPrefabPath = "Prefabs/Text/LifeBox";
    private GameObject lifeBoxPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        eventManager = EventManager.current;
        eventManager.onEscapeInput += HandleOnEscapeInput;
        eventManager.onClickButton += HandleClickButton;
        eventManager.onPlayerDie += HandlePlayerDie;
        eventManager.onDoorEnter += HandleDoorEnter;
        eventManager.onAddLife += HandleAddLife;
        lifeBoxPrefab = Resources.Load<GameObject>(lifeBoxPrefabPath);
    }

    void StartGame()
    {
        isGameStarted = true;
        playerLifes = 3;
        UpdateLifeText();
        eventManager.OpenScreen(ScreenType.LoadingScreen);
        eventManager.LoadLevel(LevelType.LevelZeroOne);
        //eventManager.LoadLevel(LevelType.LevelZeroFour);
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
            case ButtonType.ReturnMenu:
                PauseGame();
                eventManager.UnloadLevel();
                eventManager.OpenScreen(ScreenType.Menu);
                break;
        }
    }

    void HandlePlayerDie()
    {
        playerLifes -= 1;
        if (playerLifes >= 0)
        {
            UpdateLifeText();
            eventManager.OpenScreen(ScreenType.DeathScreen);
        } else
        {
            eventManager.OpenScreen(ScreenType.GameOverScreen);
        }
    }

    void HandleAddLife(Vector2 position)
    {
        playerLifes++;
        UpdateLifeText();
        if(lifeBoxPrefab != null)
        {
            Instantiate(lifeBoxPrefab, new Vector3(position.x, position.y, lifeBoxPrefab.transform.position.z), 
                Quaternion.Euler(0, 0, 0));
        }
    } 

    void UpdateLifeText()
    {
        eventManager.UpdateTextElement(UiTextElementType.Life, "x" + playerLifes);
    }

    void HandleDoorEnter(LevelType levelType)
    {
        eventManager.OpenScreen(ScreenType.LoadingScreen);
        eventManager.LoadLevel(levelType);
        eventManager.OpenScreen(ScreenType.UI);
    }
}
