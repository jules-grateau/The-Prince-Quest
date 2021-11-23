using UnityEngine;

public class PlayerMouvementController : MonoBehaviour
{
    EventManager eventManager;
    Rigidbody2D playerRb;
    GameObject lookDirectionObject;
    bool isGrounded = false;
    bool isJumping = false;
    bool isAlive = true;
    bool isDragging;

    public float jumpForce = 3;
    public float jumpTime = 0.5f;
    float jumpTimeCounter = 0f;
    public float airControlSpeed = 125;

    public float speed = 4f;

    public float hitThrowSpeed = 50f;
    public float enemyBouncingSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        lookDirectionObject = transform.GetChild(0).gameObject;
        eventManager = EventManager.current;
        eventManager.onHorizontalInput += HandleHorizontalInput;
        eventManager.onGrounded += HandleGroundedEvent;
        eventManager.onSpaceInput += HandleSpaceInput;
        eventManager.onSpaceInputDown += HandleSpaceInputDown;
        eventManager.onSpaceInputUp += HandleSpaceInputUp;
        eventManager.onEnemyCollidedWithPlayer += HandleEnemyCollidedWithPlayer;
        eventManager.onPlayerSteppedOnEnemy += HandlePlayerSteppedOnEnemy;
        eventManager.onPlayerDie += HandlePlayerDie;
        eventManager.onStopPlayerInput += HandleInputStop;
        eventManager.onStartDragging += HandleStartDragging;
        eventManager.onStopDragging += HandleStopDragging;
    }

    private void OnDestroy()
    {
        eventManager.onHorizontalInput -= HandleHorizontalInput;
        eventManager.onGrounded -= HandleGroundedEvent;
        eventManager.onSpaceInput -= HandleSpaceInput;
        eventManager.onSpaceInputDown -= HandleSpaceInputDown;
        eventManager.onSpaceInputUp -= HandleSpaceInputUp;
        eventManager.onEnemyCollidedWithPlayer -= HandleEnemyCollidedWithPlayer;
        eventManager.onPlayerSteppedOnEnemy -= HandlePlayerSteppedOnEnemy;
        eventManager.onPlayerDie -= HandlePlayerDie;
        eventManager.onStopPlayerInput -= HandleInputStop;
        eventManager.onStartDragging -= HandleStartDragging;
        eventManager.onStopDragging -= HandleStopDragging;
    }

    void HandleStartDragging(int gameObjectId)
    {
        isDragging = true;
    }

    void HandleStopDragging(int gameObjectId)
    {
        isDragging = false;
    }

    void HandleInputStop(bool shouldInputStop)
    {
        if(shouldInputStop)
        {
            playerRb.velocity = new Vector2(0, 0);
            eventManager.Walking(false);
        }
    }
    private void HandleGroundedEvent(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }

    private void HandleEnemyCollidedWithPlayer(Vector2 direction)
    {
        playerRb.AddForce((direction )* hitThrowSpeed, ForceMode2D.Impulse);
    }

    private void HandlePlayerSteppedOnEnemy(int instanceId)
    {
        playerRb.velocity = new Vector2( playerRb.velocity.x, enemyBouncingSpeed);
        eventManager.PlayerJump();
    }

    private void HandlePlayerDie()
    {
        this.isAlive = false;
    }

    private void HandleHorizontalInput(float horizontalInput)
    {
        if (!isAlive)
            return;


        float actualSpeed = speed;
        float actualAirControlSpeed = airControlSpeed;
        if (isDragging)
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

        bool isWalking = isGrounded && playerRb.velocity.x != 0;
        eventManager.Walking(isWalking);
        CalculateLookDirection(horizontalInput);
    }

    private void HandleSpaceInput()
    {
        if (!isAlive || isDragging)
            return;
        if(jumpTimeCounter <= 0)
        {
            isJumping = false;
        }

        if(isJumping)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleSpaceInputDown()
    {
        if (!isAlive || isDragging)
            return;

        if(isGrounded)
        {
            eventManager.PlayerJump();
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            isJumping = true;
            jumpTimeCounter = jumpTime;
        }
    }

    private void HandleSpaceInputUp()
    {
        isJumping = false;
        jumpTimeCounter = 0;
    }

        void CalculateLookDirection(float horizontalInput)
    {
        if (!isAlive || isDragging)
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
