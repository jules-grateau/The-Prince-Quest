using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    private EventManager eventManager;
    private Rigidbody2D playerRb;
    private BoxCollider2D boxCollider;
    private bool isAlive = true;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onPlayerDie += HandlePlayerDie;
        boxCollider = GetComponent<BoxCollider2D>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        eventManager.onPlayerDie -= HandlePlayerDie;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        CalculateGroundColision();
        CalculateEnemyColision();
        CalculateIsCrushed();
    }

    void CalculateGroundColision()
    {
        // Collider to detected collision with ground, to check if grounded
        Collider2D groundOverlapBox = Physics2D.OverlapBox(playerRb.position + Vector2.down
            , new Vector2((float) (boxCollider.size.x * 0.95), 0.10f), transform.rotation.y, groundLayer);
        bool groundHit = groundOverlapBox != null;
        eventManager.Grounded(groundHit);
    }

    void CalculateEnemyColision()
    {
        // Collider to detected collision with ground, to check if grounded
        Collider2D enemyOverlapBox = Physics2D.OverlapBox(playerRb.position + boxCollider.offset + Vector2.down
            , new Vector2(boxCollider.size.x*2, 0.10f), 0, enemyLayer);
        if (enemyOverlapBox != null)
        {
            eventManager.PlayerSteppedOnEnemy(enemyOverlapBox.gameObject.GetInstanceID());
        }
    }

    void CalculateIsCrushed()
    {
        Collider2D[] OverlapBox = Physics2D.OverlapBoxAll(playerRb.position + boxCollider.offset, boxCollider.size, 0);
        bool IsPushedLeft = false;
        bool IsPushedRight = false;
        foreach(Collider2D collider in OverlapBox)
        {
            if(collider.transform.position.x < transform.position.x)
            {
                IsPushedLeft = true;
            }
            if (collider.transform.position.x > transform.position.x)
            {
                IsPushedRight = true;
            }
        }

        if(IsPushedLeft && IsPushedRight)
        {
            eventManager.PlayerDie();
        }
    }

    void HandlePlayerDie()
    {
        isAlive = false;
    }

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.contacts[collision.contacts.Length-1].point;
        Debug.DrawLine(transform.position, contactPoint);
        if(contactPoint.x < transform.position.x)
        {
            Debug.Log("Contact a gauche");
            Debug.Log(transform.position);
            Debug.Log(contactPoint);
        }
        if(contactPoint.x > transform.position.x)
        {
            Debug.Log("Contact a droite");
            Debug.Log(transform.position);
            Debug.Log(contactPoint);
        }
    }*/
}
