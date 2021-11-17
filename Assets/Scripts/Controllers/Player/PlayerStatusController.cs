using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{
    private EventManager eventManager;
    private bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onEnemyCollidedWithPlayer += HandleEnemyCollidedWithPlayer;
        eventManager.onKillPlayer += HandleKillPlayer;
    }
    private void OnDestroy()
    {
        eventManager.onEnemyCollidedWithPlayer -= HandleEnemyCollidedWithPlayer;
        eventManager.onKillPlayer -= HandleKillPlayer;
    }

    void PlayerDie()
    {
        isAlive = false;
        eventManager.PlayerDie();
    }

    void HandleKillPlayer()
    {
        if (!isAlive)
            return;
        Debug.Log("Kill Player, Player is alive");
        PlayerDie();

    }
    void HandleEnemyCollidedWithPlayer(Vector2 direction)
    {
        if (!isAlive)
            return;

        PlayerDie();
    }
}
