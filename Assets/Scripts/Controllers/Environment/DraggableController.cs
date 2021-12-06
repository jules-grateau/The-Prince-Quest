using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Environment
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DraggableController : MonoBehaviour
    {
        PlayerEventManager _playerEventManager;
        
        Rigidbody2D _rb;
        Rigidbody2D _rbToFollow;
        float xOffset;

        Vector2 _holdDirection;

        public float velocityMultiplier = 10f;

        // Use this for initialization
        void Awake()
        {
            _playerEventManager = PlayerEventManager.current;
            _rb = GetComponent<Rigidbody2D>();
            _playerEventManager.onStartInteractWith += HandleStartInteractWith;
            _playerEventManager.onStopInteractWith += HandleStopInteractWith;
        }

        private void OnDestroy()
        {
            _playerEventManager.onStartInteractWith -= HandleStartInteractWith;
            _playerEventManager.onStopInteractWith -= HandleStopInteractWith;
        }

        void HandleStartInteractWith(GameObject interactionFrom, int gameobjectId)
        {
            if(gameObject.GetInstanceID() == gameobjectId)
            {
                StartDragging(interactionFrom);
                _playerEventManager.StartDragging(gameobjectId);
            }
        }

        void HandleStopInteractWith(int gameobjectId)
        {
            if (gameObject.GetInstanceID() == gameobjectId)
            {
                StopDragging();
                _playerEventManager.StopDragging(gameobjectId);
            }
        }

        void StartDragging(GameObject interactionFrom)
        {
            _rbToFollow = interactionFrom.GetComponent<Rigidbody2D>();
            if(_rbToFollow != null)
            {
                xOffset = _rb.position.x - _rbToFollow.position.x;
                _holdDirection = new Vector2(xOffset, 0).normalized;
            }
        }

        void StopDragging()
        {
            _rbToFollow = null;

        }

        private void FixedUpdate()
        {
            if(_rbToFollow != null)
            {
                var followDirection = new Vector2(_rbToFollow.velocity.x, 0).normalized;
                if(_holdDirection == followDirection)
                {
                    _rb.velocity = new Vector2(_rbToFollow.velocity.x * velocityMultiplier, _rb.velocity.y);
                } else
                {
                    _rb.MovePosition(new Vector2(_rbToFollow.position.x + xOffset, _rb.position.y));
                }
            }
        }
    }
}