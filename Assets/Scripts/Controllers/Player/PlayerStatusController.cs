using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{
    private EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onEnemyCollidedWithPlayer += handleEnemyCollidedWithPlayer;
    }
    private void OnDestroy()
    {
        eventManager.onEnemyCollidedWithPlayer -= handleEnemyCollidedWithPlayer;
    }

    void handleEnemyCollidedWithPlayer(Vector2 direction)
    {
        eventManager.PlayerDie();
    }
}
