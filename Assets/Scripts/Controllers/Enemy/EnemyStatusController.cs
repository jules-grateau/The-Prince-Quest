using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusController : MonoBehaviour
{
    private EventManager eventManager;
    private int instanceId;
    private bool isAlive = true;
    public int scoreValue = 50;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onPlayerSteppedOnEnemy += HandlePlayerSteppedOnEnemy;
        eventManager.onEnemyDie += HandleEnemyDie;
        instanceId = gameObject.GetInstanceID();
    }

    private void OnDestroy()
    {
        eventManager.onEnemyDie -= HandleEnemyDie;
    }

    void HandlePlayerSteppedOnEnemy(int instanceId)
    {
        if (isAlive && instanceId == this.instanceId)
        {
            eventManager.EnemyDie(this.instanceId);
            Destroy(gameObject, 2);
        }
    }

    void HandleEnemyDie(int instanceId)
    {
        if(instanceId == this.instanceId)
        {
            eventManager.AddScore(transform.position, scoreValue);
            isAlive = false;
        }
    } 
    // Update is called once per frame
    void Update()
    {
        
    }
}
