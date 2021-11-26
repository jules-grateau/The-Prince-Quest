using Assets.Scripts.Controllers.Player;
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

        bool _isGamePaused = true;
        bool _isGameStarted = false;
        int _playerLifes = 0;
        PlayerStatus _playerStatus = PlayerStatus.DefaultPlayerStatus.Clone();
        PlayerStatus _currLevelInitPlayerStatus;

        public bool isTesting = false;
        public LevelType firstLevel;
        public LevelType testLevel;
        public int initPlayerLifes = 3;

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
            _playerEventManager.onTakeBonus += HandleTakeBonus;
            _playerEventManager.onUpdatePlayerStatus += HandleUpdatePlayerStatus;

            _levelEventManager = LevelEventManager.current;
            _levelEventManager.onDoorEnter += HandleDoorEnter;
        }

        void StartGame()
        {
            _playerStatus = null;
            _currLevelInitPlayerStatus = null;
            _isGameStarted = true;
            _playerLifes = initPlayerLifes;

            UpdateLifeText();
            _uiEventManager.OpenScreen(ScreenType.LoadingScreen);

            LevelType levelToLoad = isTesting ? testLevel : firstLevel;
            _levelEventManager.LoadLevel(levelToLoad, OnLoadEndCallback);


            _uiEventManager.ActivateButton(ButtonType.RestartLevel);
            _uiEventManager.OpenScreen(ScreenType.UI);
            GameStateEventManager.current.StartGame();
            ResumeGame();
        }

        void ResumeGame()
        {
            _isGamePaused = false;
            Time.timeScale = 1;
            GameStateEventManager.current.ResumeGame();
        }

        void PauseGame()
        {
            _isGamePaused = true;
            Time.timeScale = 0;
            _uiEventManager.ActivateButton(ButtonType.ResumeGame);
            GameStateEventManager.current.PauseGame();
        }

        void RestartLevel()
        {
            _uiEventManager.OpenScreen(ScreenType.LoadingScreen);
            PauseGame();
            _levelEventManager.ReloadLevel(OnReloadLevel);
            ResumeGame();
            _uiEventManager.OpenScreen(ScreenType.UI);
        }

        void HandleOnEscapeInput()
        {
            if (_isGameStarted)
            {
                if (_isGamePaused)
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
            _playerLifes -= 1;
            if (_playerLifes >= 0)
            {
                UpdateLifeText();
                _uiEventManager.OpenScreen(ScreenType.DeathScreen);
            }
            else
            {
                _uiEventManager.OpenScreen(ScreenType.GameOverScreen);
            }
        }

        void HandleTakeBonus(Vector2 position)
        {
            if(_playerStatus.HealthPoint < PlayerStatus.DefaultPlayerStatus.HealthPoint)
            {
                _playerStatus.HealthPoint++;
                _playerEventManager.SetPlayerStatus(_playerStatus);
                _uiEventManager.UpdateLife(_playerStatus.HealthPoint);
                _uiEventManager.DisplayFloatingText(FloatingTextType.AddHealthPoint, null, position);
            } else
            {
                _playerLifes++;
                UpdateLifeText();
                _uiEventManager.DisplayFloatingText(FloatingTextType.AddLife, null, position);
            }

        }

        void HandleUpdatePlayerStatus(PlayerStatus playerStatus)
        {
            if(playerStatus != null)
            {
                _playerStatus = playerStatus;
                _uiEventManager.UpdateLife(_playerStatus.HealthPoint);
            }
        }

        void UpdateLifeText()
        {
            _uiEventManager.UpdateTextElement(UiTextElementType.Life, "x" + _playerLifes);
        }

        void HandleDoorEnter(LevelType levelType)
        {
            _uiEventManager.OpenScreen(ScreenType.LoadingScreen);
            _levelEventManager.LoadLevel(levelType, OnLoadEndCallback);
            _uiEventManager.OpenScreen(ScreenType.UI);
        }

        void OnLoadEndCallback()
        {
            _playerEventManager.SetPlayerStatus(_playerStatus);
            _currLevelInitPlayerStatus = _playerStatus?.Clone();
        }

        void OnReloadLevel()
        {
            _playerStatus = _currLevelInitPlayerStatus?.Clone();
            _playerEventManager.SetPlayerStatus(_playerStatus);
        }
    }
}