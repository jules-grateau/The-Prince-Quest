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
            , new Vector2(boxCollider.size.x * 1.25f, 0.20f), 0, enemyLayer);
        if (enemyOverlapBox != null)
        {
            eventManager.PlayerSteppedOnEnemy(enemyOverlapBox.gameObject.GetInstanceID());
        }
    }

    void HandlePlayerDie()
    {
        isAlive = false;
    }
}
