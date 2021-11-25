using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        InputEventManager _inputEventManager;
        UIEventManager _uiEventManager;
        PlayerEventManager _playerEventManager;
        LevelEventManager _levelEventManager;

        bool isGamePaused = true;
        bool isGameStarted = false;
        int playerLifes = 0;

        const string LifeBoxPrefabPath = "Prefabs/Text/LifeBox";
        GameObject lifeBoxPrefab;

        public bool isTesting = false;
        public LevelType firstLevel;
        public LevelType testLevel;

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 0;
            _inputEventManager = InputEventManager.current;
            _inputEventManager.onEscapeInput += HandleOnEscapeInput;

            _uiEventManager = UIEventManager.current;
            _uiEventManager.onClickButton += HandleClickButton;

            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onPlayerDie += HandlePlayerDie;
            _playerEventManager.onAddLife += HandleAddLife;

            _levelEventManager = LevelEventManager.current;
            _levelEventManager.onDoorEnter += HandleDoorEnter;


            lifeBoxPrefab = Resources.Load<GameObject>(LifeBoxPrefabPath);
        }

        void StartGame()
        {
            isGameStarted = true;
            playerLifes = 3;
            UpdateLifeText();
            _uiEventManager.OpenScreen(ScreenType.LoadingScreen);

            if(isTesting)
            {
                _levelEventManager.LoadLevel(testLevel);
            }
            else
            {
                _levelEventManager.LoadLevel(firstLevel);
            }


            _uiEventManager.ActivateButton(ButtonType.RestartLevel);
            _uiEventManager.OpenScreen(ScreenType.UI);
            GameStateEventManager.current.StartGame();
            ResumeGame();
        }

        void ResumeGame()
        {
            isGamePaused = false;
            Time.timeScale = 1;
            GameStateEventManager.current.ResumeGame();
        }

        void PauseGame()
        {
            isGamePaused = true;
            Time.timeScale = 0;
            _uiEventManager.ActivateButton(ButtonType.ResumeGame);
            GameStateEventManager.current.PauseGame();
        }

        void RestartLevel()
        {
            _uiEventManager.OpenScreen(ScreenType.LoadingScreen);
            PauseGame();
            _levelEventManager.ReloadLevel();
            ResumeGame();
            _uiEventManager.OpenScreen(ScreenType.UI);
        }

        void HandleOnEscapeInput()
        {
            if (isGameStarted)
            {
                if (isGamePaused)
                {
                    ResumeGame();
                    _uiEventManager.OpenScreen(ScreenType.UI);
                }
                else
                {
                    PauseGame();
                    _uiEventManager.OpenScreen(ScreenType.Menu);
                }
            }
        }

        void HandleClickButton(ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.StartGame:
                    StartGame();
                    break;
                case ButtonType.RestartLevel:
                    RestartLevel();
                    _uiEventManager.OpenScreen(ScreenType.UI);
                    break;
                case ButtonType.ResumeGame:
                    ResumeGame();
                    _uiEventManager.OpenScreen(ScreenType.UI);
                    break;
                case ButtonType.ReturnMenu:
                    PauseGame();
                    _levelEventManager.UnloadLevel();
                    _uiEventManager.OpenScreen(ScreenType.Menu);
                    break;
            }
        }

        void HandlePlayerDie()
        {
            playerLifes -= 1;
            if (playerLifes >= 0)
            {
                UpdateLifeText();
                _uiEventManager.OpenScreen(ScreenType.DeathScreen);
            }
            else
            {
                _uiEventManager.OpenScreen(ScreenType.GameOverScreen);
            }
        }

        void HandleAddLife(Vector2 position)
        {
            playerLifes++;
            UpdateLifeText();
            if (lifeBoxPrefab != null)
            {
                Instantiate(lifeBoxPrefab, new Vector3(position.x, position.y, lifeBoxPrefab.transform.position.z),
                    Quaternion.Euler(0, 0, 0));
            }
        }

        void UpdateLifeText()
        {
            _uiEventManager.UpdateTextElement(UiTextElementType.Life, "x" + playerLifes);
        }

        void HandleDoorEnter(LevelType levelType)
        {
            _uiEventManager.OpenScreen(ScreenType.LoadingScreen);
            _levelEventManager.LoadLevel(levelType);
            _uiEventManager.OpenScreen(ScreenType.UI);
        }
    }
}