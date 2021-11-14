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
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isConsumable && isConsumed)
            return;

        if(collision.CompareTag("Player"))
        {
            eventManager.TriggerAnimation(animationType);
            isConsumed = true;
        }
    }
}

