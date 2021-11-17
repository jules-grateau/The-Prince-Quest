using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KissPlayerAndLeaveAnimationScript : MonoBehaviour
{
    public string PlayerGameObjectPath = "Player";
    public float speed = 10f;
    public Vector2 moveTowardOffset;
    public Vector2 leaveTargetOffset;
    public AnimationType animationType = AnimationType.KanaKissPlayer;
    public float kissTime = 1f;

    bool kissedPlayer = false;
    bool coroutineStarted = false;
    bool animationStarted = false;

    Vector2 target;
    Vector2 leaveTarget;
    EventManager eventManager;
    Animator animator;

    private const string isWalkingParameterName = "isWalking";
    private const string isKissingParameterName = "isKissing";

    // Use this for initialization
    void Awake()
    {
        eventManager = EventManager.current;
        eventManager.onTriggerAnimation += HandleTriggerAnimation;
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        eventManager.onTriggerAnimation -= HandleTriggerAnimation;
    }

    void HandleTriggerAnimation(AnimationType animationType)
    {
        if(animationType == this.animationType)
        {
            animationStarted = true;
            CalculateAnimationData();
        }
    }

    void CalculateAnimationData()
    {
        Transform parent = transform.parent.transform;
        Transform player = parent.Find(PlayerGameObjectPath);
        target = new Vector2(player.position.x, transform.position.y) + moveTowardOffset;
        leaveTarget = (Vector2)transform.position + leaveTargetOffset;
    }
    // Update is called once per frame
    void Update()
    {
        if (!animationStarted)
            return;

        KissAndLeaveAnimation();
    }

    void KissAndLeaveAnimation()
    {
        eventManager.StartAnimation(AnimationType.KanaKissPlayer);
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);
        bool reachedTarget = (Vector2)transform.position == target;
        animator.SetBool(isWalkingParameterName, !reachedTarget);

        if (reachedTarget)
        {
            if (!kissedPlayer && !coroutineStarted)
            {
                eventManager.IsKissing(true);
                animator.SetBool(isKissingParameterName, true);
                StartCoroutine(StopKissAndLeave());
            } else if (kissedPlayer)
            {
                eventManager.StopAnimation(AnimationType.KanaKissPlayer);
                Destroy(gameObject);
            }

        }
    }

    IEnumerator StopKissAndLeave()
    {
        coroutineStarted = true;
        yield return new WaitForSecondsRealtime(kissTime);
        eventManager.IsKissing(false);
        animator.SetBool(isKissingParameterName, false);
        transform.eulerAngles = transform.eulerAngles - new Vector3(0, 180, 0);
        target = leaveTarget;
        kissedPlayer = true;
    }
}
