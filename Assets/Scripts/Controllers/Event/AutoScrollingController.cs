using Assets.Scripts.Enum;
using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Controllers.Event
{
    public class AutoScrollingController : MonoBehaviour
    {
        public Vector2[] targets;
        private Vector3 currentTarget;
        private int currentIndex = 0;
        public float speed = 0.5f;
        bool isEventStarted = false;
        EventManager eventManager;
        InGameEventType gameEvent = InGameEventType.AutoScrollingStart;

        private void Awake()
        {
            eventManager = EventManager.current;
            eventManager.onStartGameEvent += HandleStartGameEvent;
            CalculateCurrentTarget();
        }

        private void OnDestroy()
        {
            eventManager.onStartGameEvent -= HandleStartGameEvent;
        }

        void HandleStartGameEvent(InGameEventType gameEventType)
        {
            if (gameEventType == gameEvent)
            {
                isEventStarted = true;
                Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    collider.enabled = true;
                }
            }
        }

        void OnReachedEndScolling()
        {
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
                isEventStarted = false;
            }
        }

        void CalculateCurrentTarget()
        {
            Vector2 target = targets[currentIndex];
            currentTarget = new Vector3(target.x, target.y, transform.position.z);
        }


        // Update is called once per frame
        void Update()
        {
            if (!isEventStarted)
                return;

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);

            if (transform.position == currentTarget)
            {
                currentIndex++;

                if (currentIndex < targets.Length)
                {
                    CalculateCurrentTarget();

                }
                else
                {
                    OnReachedEndScolling();
                }
            }
        }
    }
}