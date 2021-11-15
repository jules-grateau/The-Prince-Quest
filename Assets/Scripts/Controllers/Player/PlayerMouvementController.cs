using UnityEngine;

public class PlayerMouvementController : MonoBehaviour
{
    private EventManager eventManager;
    private Rigidbody2D playerRb;
    private GameObject lookDirectionObject;
    private bool isGrounded;
    private bool isAlive = true;
    private float forceAdded = 0;

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
        playerRb.AddForce(Vector2.up * enemyBouncingSpeed, ForceMode2D.Impulse);
    }

    private void handlePlayerDie()
    {
        this.isAlive = false;
    }

    private void handleHorizontalInput(float horizontalInput)
    {
        if (!isAlive)
            return;

        if (isGrounded)
        {
            //We give a specific velocity to the character when on ground
            playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);
        }
        else
        {
            //But air control is done by adding force
            playerRb.AddForce(Vector2.right * horizontalInput * airControlSpeed * Time.deltaTime, ForceMode2D.Impulse);
            //Controling that airspeed does not get higher than ground speed and force is added
            if (Mathf.Abs(playerRb.velocity.x) > speed)
            {
                playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);
            }
        }

        bool isWalking = isGrounded && playerRb.velocity.x != 0;
        eventManager.Walking(isWalking);
        CalculateLookDirection(horizontalInput);
    }
    private void handleSpaceInput()
    {
        if (!isAlive)
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
        if (!isAlive)
            return;

        if (isGrounded)
        {
            Vector2 forceToAdd = Vector2.up * initJumpForce;
            playerRb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

        void CalculateLookDirection(float horizontalInput)
    {
        if (!isAlive)
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
