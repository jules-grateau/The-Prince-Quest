using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Event
{
    public class AutoScrollingController : MonoBehaviour
    {
        public Vector2[] targets;

        Vector3 _currentTarget;
        int _currentIndex = 0;
        float _speed = 0.5f;
        
        bool _isEventStarted = false;
        
        LevelEventManager _levelEventManager;
        
        InGameEventType gameEvent = InGameEventType.AutoScrollingStart;

        private void Awake()
        {
            _levelEventManager = LevelEventManager.current;
            _levelEventManager.onStartGameEvent += HandleStartGameEvent;
            CalculateCurrentTarget();
        }

        private void OnDestroy()
        {
            _levelEventManager.onStartGameEvent -= HandleStartGameEvent;
        }

        void HandleStartGameEvent(InGameEventType gameEventType)
        {
            if (gameEventType == gameEvent)
            {
                _isEventStarted = true;
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
                _isEventStarted = false;
            }
        }

        void CalculateCurrentTarget()
        {
            Vector2 target = targets[_currentIndex];
            _currentTarget = new Vector3(target.x, target.y, transform.position.z);
        }


        // Update is called once per frame
        void Update()
        {
            if (!_isEventStarted)
                return;

            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget, step);

            if (transform.position == _currentTarget)
            {
                _currentIndex++;

                if (_currentIndex < targets.Length)
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