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
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onWalking += setWalkingParameter;
        eventManager.onGrounded += setGroundedParameter;
        eventManager.onPlayerDie += setIsAliveParameterFalse;
        animator = GetComponent<Animator>();
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
