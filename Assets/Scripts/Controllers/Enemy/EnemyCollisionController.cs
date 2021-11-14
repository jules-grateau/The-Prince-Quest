using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionController : MonoBehaviour
{
    private EventManager eventManager;
    private int instanceId;
    private bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        instanceId = gameObject.GetInstanceID();
        eventManager.onEnemyDie += handleEnemyDie;
    }

    private void OnDestroy()
    {
        eventManager.onEnemyDie -= handleEnemyDie;
    }


    void handleEnemyDie(int instanceId)
    {
        if(instanceId == this.instanceId)
        {
            isAlive = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive)
            return;

        GameObject objectCollided = collision.gameObject;
        if (objectCollided.CompareTag("Player"))
        {
            Vector2 collisionDirection = objectCollided.transform.position - transform.position;
            eventManager.EnemyCollidedWithPlayer(collisionDirection.normalized);
        }
    }
}
