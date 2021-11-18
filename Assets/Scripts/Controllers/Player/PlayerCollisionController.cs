using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    EventManager eventManager;
    Rigidbody2D playerRb;
    BoxCollider2D boxCollider;
    bool isAlive = true;
    GameObject lookDirectionObject;

    public float interacteDistance = 0.5f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onPlayerDie += HandlePlayerDie;
        boxCollider = GetComponent<BoxCollider2D>();
        playerRb = GetComponent<Rigidbody2D>();
        lookDirectionObject = transform.GetChild(0).gameObject;
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
        CalculateInteractable();
    }

    void CalculateInteractable()
    {
        Vector2 direction = lookDirectionObject.transform.position.x - transform.position.x < 0 ? Vector2.left : Vector2.right;

        RaycastHit2D interactableOverlapRay = Physics2D.Raycast(transform.position, direction, interacteDistance, groundLayer);
        Debug.DrawRay(transform.position, direction, Color.red);
        Collider2D collider = interactableOverlapRay.collider;

        if (collider != null && collider.CompareTag("Interactable"))
        {
            Debug.DrawRay(transform.position, direction , Color.green);
            eventManager.CanInteractWith(collider.gameObject.GetInstanceID());
        } else
        {
            eventManager.CanInteractWith(0);
        }
    }

    void CalculateGroundColision()
    {
        Vector2 boxPoint = playerRb.position + new Vector2(boxCollider.offset.x, -boxCollider.size.y );
        Vector2 boxSize = new Vector2((boxCollider.size.x * 0.95f), 0.10f);
        // Collider to detected collision with ground, to check if grounded
        Collider2D groundOverlapBox = Physics2D.OverlapBox(boxPoint
            , boxSize, transform.rotation.y, groundLayer);
        bool groundHit = groundOverlapBox != null;
        Debug.DrawRay(boxPoint, boxSize, groundHit ? Color.green : Color.red);
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
