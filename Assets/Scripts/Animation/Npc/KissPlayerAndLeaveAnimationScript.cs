using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Animation.Npc
{
    public class KissPlayerAndLeaveAnimationScript : MonoBehaviour
    {
        public string playerGameObjectPath = "Player";
        public float speed = 10f;
        public Vector2 moveTowardOffset;
        public Vector2 leaveTargetOffset;
        public AnimationType animationType = AnimationType.KanaKissPlayer;
        public float kissTime = 1f;

        bool _kissedPlayer = false;
        bool _coroutineStarted = false;
        bool _animationStarted = false;

        Vector2 _target;
        Vector2 _leaveTarget;
        AnimationEventManager _animationEventManager;
        Animator _animator;

        private const string IsWalkingParameterName = "isWalking";
        private const string IsKissingParameterName = "isKissing";

        // Use this for initialization
        void Awake()
        {
            _animationEventManager = AnimationEventManager.current;
            _animationEventManager.onTriggerAnimation += HandleTriggerAnimation;
            _animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            _animationEventManager.onTriggerAnimation -= HandleTriggerAnimation;
        }

        void HandleTriggerAnimation(AnimationType animationType)
        {
            if (animationType == this.animationType)
            {
                _animationStarted = true;
                CalculateAnimationData();
            }
        }

        void CalculateAnimationData()
        {
            Transform parent = transform.parent.transform;
            Transform player = parent.Find(playerGameObjectPath);
            _target = new Vector2(player.position.x, transform.position.y) + moveTowardOffset;
            _leaveTarget = (Vector2)transform.position + leaveTargetOffset;
        }
        // Update is called once per frame
        void Update()
        {
            if (!_animationStarted)
                return;

            KissAndLeaveAnimation();
        }

        void KissAndLeaveAnimation()
        {
            _animationEventManager.StartAnimation(AnimationType.KanaKissPlayer);
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _target, step);
            bool reachedTarget = (Vector2)transform.position == _target;
            _animator.SetBool(IsWalkingParameterName, !reachedTarget);

            if (reachedTarget)
            {
                if (!_kissedPlayer && !_coroutineStarted)
                {
                    PlayerEventManager.current.IsKissing(true);
                    _animator.SetBool(IsKissingParameterName, true);
                    StartCoroutine(StopKissAndLeave());
                }
                else if (_kissedPlayer)
                {
                    _animationEventManager.StopAnimation(AnimationType.KanaKissPlayer);
                    Destroy(gameObject);
                }

            }
        }

        IEnumerator StopKissAndLeave()
        {
            _coroutineStarted = true;
            yield return new WaitForSecondsRealtime(kissTime);
            PlayerEventManager.current.IsKissing(false);
            _animator.SetBool(IsKissingParameterName, false);
            transform.eulerAngles = transform.eulerAngles - new Vector3(0, 180, 0);
            _target = _leaveTarget;
            _kissedPlayer = true;
        }
    }
}