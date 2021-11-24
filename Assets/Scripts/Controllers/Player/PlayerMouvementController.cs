using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerMouvementController : MonoBehaviour
    {
        PlayerEventManager _playerEventManager;
        InputEventManager _inputEventManager;
        EnemyEventManager _enemyEventManager;

        Rigidbody2D _playerRb;
        GameObject _lookDirectionObject;
        bool _isGrounded = false;
        bool _isJumping = false;
        bool _isAlive = true;
        bool _isDragging;

        public float jumpForce = 3;
        public float jumpTime = 0.5f;
        float _jumpTimeCounter = 0f;

        public float airControlSpeed = 125;
        public float speed = 4f;
        public float hitThrowSpeed = 50f;
        public float enemyBouncingSpeed = 3f;

        // Start is called before the first frame update
        void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _lookDirectionObject = transform.GetChild(0).gameObject;

            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onGrounded += HandleGroundedEvent;
            _playerEventManager.onPlayerSteppedOnEnemy += HandlePlayerSteppedOnEnemy;
            _playerEventManager.onPlayerDie += HandlePlayerDie;
            _playerEventManager.onStartDragging += HandleStartDragging;
            _playerEventManager.onStopDragging += HandleStopDragging;

            _inputEventManager = InputEventManager.current;
            _inputEventManager.onHorizontalInput += HandleHorizontalInput;
            _inputEventManager.onSpaceInput += HandleSpaceInput;
            _inputEventManager.onSpaceInputDown += HandleSpaceInputDown;
            _inputEventManager.onSpaceInputUp += HandleSpaceInputUp;
            _inputEventManager.onStopPlayerInput += HandleInputStop;

            _enemyEventManager = EnemyEventManager.current;
            _enemyEventManager.onEnemyCollidedWithPlayer += HandleEnemyCollidedWithPlayer;
        }

        private void OnDestroy()
        {
            _playerEventManager.onGrounded -= HandleGroundedEvent;
            _playerEventManager.onPlayerSteppedOnEnemy -= HandlePlayerSteppedOnEnemy;
            _playerEventManager.onPlayerDie -= HandlePlayerDie;
            _playerEventManager.onStartDragging -= HandleStartDragging;
            _playerEventManager.onStopDragging -= HandleStopDragging;

            _inputEventManager.onHorizontalInput -= HandleHorizontalInput;
            _inputEventManager.onSpaceInput -= HandleSpaceInput;
            _inputEventManager.onSpaceInputDown -= HandleSpaceInputDown;
            _inputEventManager.onSpaceInputUp -= HandleSpaceInputUp;
            _inputEventManager.onStopPlayerInput -= HandleInputStop;

            _enemyEventManager.onEnemyCollidedWithPlayer -= HandleEnemyCollidedWithPlayer;
        }

        void HandleStartDragging(int gameObjectId)
        {
            _isDragging = true;
        }

        void HandleStopDragging(int gameObjectId)
        {
            _isDragging = false;
        }

        void HandleInputStop(bool shouldInputStop)
        {
            if (shouldInputStop)
            {
                _playerRb.velocity = new Vector2(0, 0);
                _playerEventManager.Walking(false);
            }
        }
        private void HandleGroundedEvent(bool isGrounded)
        {
            this._isGrounded = isGrounded;
        }

        private void HandleEnemyCollidedWithPlayer(Vector2 direction)
        {
            _playerRb.AddForce(direction * hitThrowSpeed, ForceMode2D.Impulse);
        }

        private void HandlePlayerSteppedOnEnemy(int instanceId)
        {
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, enemyBouncingSpeed);
            _playerEventManager.PlayerJump();
        }

        private void HandlePlayerDie()
        {
            _isAlive = false;
        }

        private void HandleHorizontalInput(float horizontalInput)
        {
            if (!_isAlive)
                return;


            float actualSpeed = speed;
            float actualAirControlSpeed = airControlSpeed;
            if (_isDragging)
            {
                actualSpeed = speed / 2;
                actualAirControlSpeed = airControlSpeed / 2;
            }

            if (_isGrounded)
            {
                //We give a specific velocity to the character when on ground
                _playerRb.velocity = new Vector2(horizontalInput * actualSpeed, _playerRb.velocity.y);
            }
            else
            {

                //But air control is done by adding force
                _playerRb.AddForce(Vector2.right * horizontalInput * actualAirControlSpeed * Time.deltaTime, ForceMode2D.Impulse);
                //Controling that airspeed does not get higher than ground speed and force is added
                if (Mathf.Abs(_playerRb.velocity.x) > speed)
                {
                    _playerRb.velocity = new Vector2(horizontalInput * actualSpeed, _playerRb.velocity.y);
                }
            }

            bool isWalking = _isGrounded && _playerRb.velocity.x != 0;
            _playerEventManager.Walking(isWalking);
            CalculateLookDirection(horizontalInput);
        }

        private void HandleSpaceInput()
        {
            if (!_isAlive || _isDragging)
                return;
            if (_jumpTimeCounter <= 0)
            {
                _isJumping = false;
            }

            if (_isJumping)
            {
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, jumpForce);
                _jumpTimeCounter -= Time.deltaTime;
            }
        }

        private void HandleSpaceInputDown()
        {
            if (!_isAlive || _isDragging)
                return;

            if (_isGrounded)
            {
                _playerEventManager.PlayerJump();
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, jumpForce);
                _isJumping = true;
                _jumpTimeCounter = jumpTime;
            }
        }

        private void HandleSpaceInputUp()
        {
            _isJumping = false;
            _jumpTimeCounter = 0;
        }

        void CalculateLookDirection(float horizontalInput)
        {
            if (!_isAlive || _isDragging)
                return;

            // Using the LookDirectionObject to check where the character is looking at
            // And rotating the character on the Y axis if he change direction
            Vector2 lookDirection = _lookDirectionObject.transform.position - transform.position;

            if (horizontalInput < 0 && lookDirection.x > 0)
            {
                _playerRb.transform.eulerAngles = _playerRb.transform.eulerAngles + new Vector3(0, 180, 0);
            }

            if (horizontalInput > 0 && lookDirection.x < 0)
            {
                _playerRb.transform.eulerAngles = _playerRb.transform.eulerAngles - new Vector3(0, 180, 0);
            }
        }
    }
}