using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{
    EventManager eventManager;
    bool isAlive = true;
    int canInteractWith;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onEnemyCollidedWithPlayer += HandleEnemyCollidedWithPlayer;
        eventManager.onKillPlayer += HandleKillPlayer;
        eventManager.onCanInteractWith += HandleCanInteractWith;
        eventManager.onInteractKeyDown += HandleInteractKeyDown;
        eventManager.onInteractKeyUp += HandleInteractKeyUp;
    }
    void OnDestroy()
    {
        eventManager.onEnemyCollidedWithPlayer -= HandleEnemyCollidedWithPlayer;
        eventManager.onKillPlayer -= HandleKillPlayer;
        eventManager.onCanInteractWith -= HandleCanInteractWith;
        eventManager.onInteractKeyDown -= HandleInteractKeyDown;
        eventManager.onInteractKeyUp -= HandleInteractKeyUp;
    }

    void HandleCanInteractWith(int gameobjectId)
    {
        if(gameobjectId == 0 && canInteractWith != 0)
        {
            eventManager.StopInteractWith(canInteractWith);
        }

        canInteractWith = gameobjectId;
    }

    void HandleInteractKeyDown()
    {
        Debug.Log($"HandleInteractKeyDown With : {canInteractWith}");
        if (canInteractWith != 0)
        {
            eventManager.StartInteractWith(canInteractWith);
        }
    }

    void HandleInteractKeyUp()
    {
        if (canInteractWith != 0)
        {
            eventManager.StopInteractWith(canInteractWith);
        }
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

        PlayerDie();

    }
    void HandleEnemyCollidedWithPlayer(Vector2 direction)
    {
        if (!isAlive)
            return;

        PlayerDie();
    }
}
