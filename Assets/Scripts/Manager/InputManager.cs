using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class InputManager : MonoBehaviour
    {
        private GameStateEventManager _gameStateEventManager;
        private InputEventManager _inputEventManager;
        private bool _isGamePaused = true;
        private bool _isInputStopped = false;

        // Start is called before the first frame update
        void Start()
        {
            _gameStateEventManager = GameStateEventManager.current;
            _gameStateEventManager.onResumeGame += HandleResumeGame;
            _gameStateEventManager.onPauseGame += HandlePauseGame;

            _inputEventManager = InputEventManager.current;
            _inputEventManager.onStopPlayerInput += HandleInputStop;
        }

        private void OnDestroy()
        {
            _gameStateEventManager.onResumeGame -= HandleResumeGame;
            _gameStateEventManager.onPauseGame -= HandlePauseGame;

            _inputEventManager.onStopPlayerInput -= HandleInputStop;
        }

        void HandleInputStop(bool shouldInputStop)
        {
            _isInputStopped = shouldInputStop;
        }

        void HandleResumeGame()
        {
            _isGamePaused = false;
        }

        void HandlePauseGame()
        {
            _isGamePaused = true;
        }


        private void Update()
        {
            if (_isInputStopped)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _inputEventManager.EscapeInput();
            }

            if (_isGamePaused)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _inputEventManager.SpaceInputDown();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _inputEventManager.SpaceInputUp();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _inputEventManager.InteractKeyDown();
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                _inputEventManager.InteractKeyUp();
            }
        }

        void FixedUpdate()
        {
            if (_isInputStopped)
                return;

            if (_isGamePaused)
                return;

            _inputEventManager.HorizontalInput(Input.GetAxisRaw("Horizontal"));

            if (Input.GetKey(KeyCode.Space))
            {
                _inputEventManager.SpaceInput();
            }
        }
    }
}