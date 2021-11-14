using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private EventManager eventManager;
    private Animator animator;

    private const string isWalkingParameterName = "isWalking";
    private const string isGroundedParameterName = "isGrounded";
    private const string isAliveParameterName = "isAlive";
    private const string isKissingParameterName = "isKissing";
    private const string kissingAnimationName = "PlayerKiss";

    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onWalking += setWalkingParameter;
        eventManager.onGrounded += setGroundedParameter;
        eventManager.onPlayerDie += setIsAliveParameterFalse;
        eventManager.onIsKissing += setKissingParameter;
        animator = GetComponent<Animator>();
    }

    void setKissingParameter(bool isKissing)
    {
        animator.SetBool(isKissingParameterName, isKissing);
    }

    private void OnDestroy()
    {
        eventManager.onWalking -= setWalkingParameter;
        eventManager.onGrounded -= setGroundedParameter;
        eventManager.onPlayerDie -= setIsAliveParameterFalse;
    }

    void setIsAliveParameterFalse()
    {
        animator.SetBool(isAliveParameterName, false);
    }
    void setWalkingParameter(bool isWalking)
    {
        animator.SetBool(isWalkingParameterName, isWalking);
    }

    void setGroundedParameter(bool isGrounded)
    {
        animator.SetBool(isGroundedParameterName, isGrounded);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
