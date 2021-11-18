using UnityEngine;

public class PlayerMouvementController : MonoBehaviour
{
    EventManager eventManager;
    Rigidbody2D playerRb;
    GameObject lookDirectionObject;
    bool isGrounded;
    bool isAlive = true;
    float forceAdded = 0;
    GameObject gameObjectDragged = null;
    float gameObjectDraggedOffsetX;


    public float maxJumpForce = 80;
    public float initJumpForce = 80;
    public float jumpForce = 8;
    public float speed = 4f;
    public float airControlSpeed = 125;
    public float jumpVelocity = 5.5f;
    public float hitThrowSpeed = 50f;
    public float enemyBouncingSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        lookDirectionObject = transform.GetChild(0).gameObject;
        eventManager = EventManager.current;
        eventManager.onHorizontalInput += handleHorizontalInput;
        eventManager.onGrounded += handleGroundedEvent;
        eventManager.onSpaceInput += handleSpaceInput;
        eventManager.onSpaceInputDown += handleSpaceInputDown;
        eventManager.onEnemyCollidedWithPlayer += handleEnemyCollidedWithPlayer;
        eventManager.onPlayerSteppedOnEnemy += handlePlayerSteppedOnEnemy;
        eventManager.onPlayerDie += handlePlayerDie;
        eventManager.onStopPlayerInput += HandleInputStop;
        eventManager.onStartDragging += HandleStartDragging;
        eventManager.onStopDragging += HandleStopDragging;
    }

    private void OnDestroy()
    {
        eventManager.onHorizontalInput -= handleHorizontalInput;
        eventManager.onGrounded -= handleGroundedEvent;
        eventManager.onSpaceInput -= handleSpaceInput;
        eventManager.onSpaceInputDown -= handleSpaceInputDown;
        eventManager.onEnemyCollidedWithPlayer -= handleEnemyCollidedWithPlayer;
        eventManager.onPlayerSteppedOnEnemy -= handlePlayerSteppedOnEnemy;
        eventManager.onPlayerDie -= handlePlayerDie;
        eventManager.onStopPlayerInput -= HandleInputStop;
        eventManager.onStartDragging -= HandleStartDragging;
        eventManager.onStopDragging -= HandleStopDragging;
    }

    void HandleStartDragging(GameObject gameObject)
    {
        gameObjectDragged = gameObject;
        gameObjectDraggedOffsetX = gameObject.transform.position.x - transform.position.x;
    }

    void HandleStopDragging(GameObject gameObject)
    {
        if(gameObjectDragged != null && gameObjectDragged.GetInstanceID() == gameObject.GetInstanceID())
        {
            gameObjectDragged = null;
        }
    }

    void HandleInputStop(bool shouldInputStop)
    {
        if(shouldInputStop)
        {
            playerRb.velocity = new Vector2(0, 0);
            eventManager.Walking(false);
        }
    }
    private void handleGroundedEvent(bool isGrounded)
    {
        this.isGrounded = isGrounded;
        if (isGrounded)
        {
            forceAdded = 0;
        }
    }

    private void handleEnemyCollidedWithPlayer(Vector2 direction)
    {
        playerRb.AddForce((direction )* hitThrowSpeed, ForceMode2D.Impulse);
    }

    private void handlePlayerSteppedOnEnemy(int instanceId)
    {
        playerRb.velocity = new Vector2( playerRb.velocity.x, enemyBouncingSpeed);
        eventManager.PlayerJump();
    }

    private void handlePlayerDie()
    {
        this.isAlive = false;
    }

    bool IsDragging()
    {
        return gameObjectDragged != null;
    }

    private void handleHorizontalInput(float horizontalInput)
    {
        if (!isAlive)
            return;


        float actualSpeed = speed;
        float actualAirControlSpeed = airControlSpeed;
        if (IsDragging())
        {
            actualSpeed = speed / 2;
            actualAirControlSpeed = airControlSpeed / 2;
        }

        if (isGrounded)
        {
            //We give a specific velocity to the character when on ground
            playerRb.velocity = new Vector2(horizontalInput * actualSpeed, playerRb.velocity.y);
        }
        else
        {

            //But air control is done by adding force
            playerRb.AddForce(Vector2.right * horizontalInput * actualAirControlSpeed * Time.deltaTime, ForceMode2D.Impulse);
            //Controling that airspeed does not get higher than ground speed and force is added
            if (Mathf.Abs(playerRb.velocity.x) > speed)
            {
                playerRb.velocity = new Vector2(horizontalInput * actualSpeed, playerRb.velocity.y);
            }
        }

        if(gameObjectDragged != null)
        {
            gameObjectDragged.transform.position = new Vector3(transform.position.x + gameObjectDraggedOffsetX, gameObjectDragged.transform.position.y);
        }

        bool isWalking = isGrounded && playerRb.velocity.x != 0;
        eventManager.Walking(isWalking);
        CalculateLookDirection(horizontalInput);
    }
    private void handleSpaceInput()
    {
        if (!isAlive || IsDragging())
            return;

        if (!isGrounded && playerRb.velocity.y > 0 && forceAdded < maxJumpForce)
        {
            Vector2 forceToAdd = Vector2.up * jumpForce;
            playerRb.AddForce(forceToAdd, ForceMode2D.Impulse);
            forceAdded += forceToAdd.y;
        }
    }

    private void handleSpaceInputDown()
    {
        if (!isAlive || IsDragging())
            return;

        if (isGrounded)
        {
            eventManager.PlayerJump();
            Vector2 forceToAdd = Vector2.up * initJumpForce;
            playerRb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

        void CalculateLookDirection(float horizontalInput)
    {
        if (!isAlive || IsDragging())
            return;

        // Using the LookDirectionObject to check where the character is looking at
        // And rotating the character on the Y axis if he change direction
        Vector2 lookDirection = lookDirectionObject.transform.position - transform.position;

        if (horizontalInput < 0 && lookDirection.x > 0)
        {
            playerRb.transform.eulerAngles = playerRb.transform.eulerAngles + new Vector3(0, 180, 0);
        }

        if (horizontalInput > 0 && lookDirection.x < 0)
        {
            playerRb.transform.eulerAngles = playerRb.transform.eulerAngles - new Vector3(0, 180, 0);
        }
    }
}
