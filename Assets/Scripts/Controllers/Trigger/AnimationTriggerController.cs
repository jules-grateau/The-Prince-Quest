using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerController : MonoBehaviour
{
    public AnimationType animationType;
    public bool isConsumable = true;
    EventManager eventManager;
    private bool isConsumed = false;
    private bool isPlayerDead = false;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onPlayerDie += handlePlayerDie;
    }

    private void OnDestroy()
    {
        eventManager.onPlayerDie -= handlePlayerDie;
    }

    void handlePlayerDie()
    {
        isPlayerDead = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerDead)
            return;
        if (isConsumable && isConsumed)
            return;

        if(collision.CompareTag("Player"))
        {
            eventManager.TriggerAnimation(animationType);
            isConsumed = true;
        }
    }
}

