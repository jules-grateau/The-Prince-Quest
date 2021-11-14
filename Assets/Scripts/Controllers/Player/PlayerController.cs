using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator animator;
    private BoxCollider2D boxCollider;
    public float speed = 5f;
    public float airControlSpeed = 5f;
    public float jumpVelocity = 2.5f;
    public float enemyBouncingSpeed = 100f;
    public float hitThrowSpeed = 200f; 
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    public GameObject lookDirectionObject;
    public Vector3 lookDirection;

    public bool isGrounded = false;
    public bool isWalking = false;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            HandleEnemyColision();
            //CalculateGroundColision();
            //CalculateLookDirection(horizontalInput);
            //HandlePlayerMovement(horizontalInput);
        }
  
    }

    void CalculateGroundColision()
    {
        // Collider to detected collision with ground, to check if grounded
        Collider2D groundOverlapBox = Physics2D.OverlapBox(playerRb.position + boxCollider.offset + Vector2.down
            , new Vector2(boxCollider.size.x, 0.10f), transform.rotation.y, groundLayer);
        bool groundHit = groundOverlapBox != null;
        isGrounded = groundHit;
        animator.SetBool("isGrounded", isGrounded);
    }

    void HandleEnemyColision()
    {
        // Collider to detected collision with ground, to check if grounded
        Collider2D enemyOverlapBox = Physics2D.OverlapBox(playerRb.position + boxCollider.offset + Vector2.down
            , new Vector2(boxCollider.size.x, 0.10f), 0, enemyLayer);
        if(enemyOverlapBox != null)
        {
            EnemyController enemy = enemyOverlapBox.gameObject.GetComponent<EnemyController>();
            enemy.Kill();
            playerRb.AddForce(Vector2.up * enemyBouncingSpeed, ForceMode2D.Impulse);
        }
    }
    /*void CalculateLookDirection(float horizontalInput)
    {
        // Using the LookDirectionObject to check where the character is looking at
        // And rotating the character on the Y axis if he change direction
        lookDirection = lookDirectionObject.transform.position - transform.position;

        if (horizontalInput < 0 && lookDirection.x > 0)
        {
            playerRb.transform.eulerAngles = playerRb.transform.eulerAngles + new Vector3(0, 180, 0);
        }

        if (horizontalInput > 0 && lookDirection.x < 0)
        {
            playerRb.transform.eulerAngles = playerRb.transform.eulerAngles - new Vector3(0, 180, 0);
        }
    }*/

    void HandlePlayerMovement(float horizontalInput)
    {
        /*if (isGrounded)
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
        }*/

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpVelocity);
        }

        isWalking = isGrounded && playerRb.velocity.x != 0;
        animator.SetBool("isWalking", isWalking);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject objectCollided = collision.gameObject;
        if (objectCollided.CompareTag("Enemy") && objectCollided.GetComponent<EnemyController>().isAlive)
        {

            isAlive = false;
            animator.SetBool("isAlive", false);
            Vector2 collisionDirection =  objectCollided.transform.position - transform.position;
            if(collisionDirection.x < 0)
            {
                playerRb.AddForce(Vector2.right * hitThrowSpeed, ForceMode2D.Impulse);
            } else
            {
                playerRb.AddForce(Vector2.left * hitThrowSpeed, ForceMode2D.Impulse);
            }

        }
    }
}
