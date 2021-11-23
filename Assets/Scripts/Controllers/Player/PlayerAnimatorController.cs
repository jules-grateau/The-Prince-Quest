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
    private const string isDraggingParameterName = "isDragging";

    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onWalking += setWalkingParameter;
        eventManager.onGrounded += setGroundedParameter;
        eventManager.onPlayerDie += setIsAliveParameterFalse;
        eventManager.onIsKissing += setKissingParameter;
        eventManager.onStartDragging += setDragParameterTrue;
        eventManager.onStopDragging += setDragParameterFalse;
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        eventManager.onWalking -= setWalkingParameter;
        eventManager.onGrounded -= setGroundedParameter;
        eventManager.onPlayerDie -= setIsAliveParameterFalse;
        eventManager.onIsKissing -= setKissingParameter;
        eventManager.onStartDragging -= setDragParameterTrue;
        eventManager.onStopDragging -= setDragParameterFalse;
    }


    void setDragParameterTrue(int gameObjectId)
    {
        animator.SetBool(isDraggingParameterName, true);
    }

    void setDragParameterFalse(int gameObjectId)
    {
        animator.SetBool(isDraggingParameterName, false);
    }

    void setKissingParameter(bool isKissing)
    {
        animator.SetBool(isKissingParameterName, isKissing);
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
