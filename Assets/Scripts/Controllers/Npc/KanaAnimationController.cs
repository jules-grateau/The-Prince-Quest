using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanaAnimationController : MonoBehaviour
{
    public AnimationType animationType;
    Animator animator;
    EventManager eventManager;
    // Start is called before the first frame update
    void Awake()
    {
        eventManager = EventManager.current;
        animator = GetComponent<Animator>();
    }

    void StartAnimation()
    {
        eventManager.StartAnimation(animationType);
    }

    void StopAnimation()
    {
        animator.enabled = false;
        Destroy(gameObject);
        eventManager.StopAnimation(animationType);
    }

    void StartKissing()
    {
        eventManager.IsKissing(true);
    }

    void StopKissing()
    {
        eventManager.IsKissing(false);
    }
}
