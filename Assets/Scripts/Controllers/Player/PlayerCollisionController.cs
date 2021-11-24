using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    EventManager eventManager;
    Rigidbody2D playerRb;
    BoxCollider2D boxCollider;
    bool isAlive = true;
    bool isDragging = false;
    GameObject lookDirectionObject;

    public float groundCollisionWidthMultiplier = 0.95f;
    public float groundCollisionHeight = 0.10f;
    public float interacteDistance = 0.5f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onPlayerDie += HandlePlayerDie;
        eventManager.onStartDragging += HandleStartDragging;
        eventManager.onStopDragging += HandleStopDragging;
        boxCollider = GetComponent<BoxCollider2D>();
        playerRb = GetComponent<Rigidbody2D>();
        lookDirectionObject = transform.GetChild(0).gameObject;
    }

    private void OnDestroy()
    {
        eventManager.onPlayerDie -= HandlePlayerDie;
        eventManager.onStartDragging -= HandleStartDragging;
        eventManager.onStartDragging -= HandleStopDragging;
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
        float actualInteractDistance = interacteDistance;
        if(isDragging)
        {
            actualInteractDistance *= 2;
        }
        RaycastHit2D interactableOverlapRay = Physics2D.Raycast(transform.position, direction, actualInteractDistance, groundLayer);
        Collider2D collider = interactableOverlapRay.collider;

        if (collider != null && collider.CompareTag("Interactable"))
        {
            eventManager.CanInteractWith(collider.gameObject.GetInstanceID());
        } else
        {
            eventManager.CanInteractWith(0);
        }

        Debug.DrawRay(transform.position, direction * actualInteractDistance, collider != null && collider.CompareTag("Interactable") ? Color.green : Color.red);
    }

    void CalculateGroundColision()
    {
        Vector2 boxPoint = playerRb.position + new Vector2(boxCollider.offset.x, -boxCollider.size.y );
        Vector2 boxSize = new Vector2((boxCollider.size.x * groundCollisionWidthMultiplier), groundCollisionHeight);
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
            if (collider.isTrigger)
                return;

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

    void HandleStartDragging(int goId)
    {
        isDragging = true;
    }

    void HandleStopDragging(int goId)
    {
        isDragging = false;
    }
}
