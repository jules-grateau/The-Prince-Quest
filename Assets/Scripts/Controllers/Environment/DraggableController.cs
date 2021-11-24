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
        
        Vector2 _positionOffset;
        Vector2 _draggingDirection;

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
                _rbToFollow = null;
                _playerEventManager.StopDragging(gameobjectId);
            }
        }

        void StartDragging(GameObject interactionFrom)
        {
            _rbToFollow = interactionFrom.GetComponent<Rigidbody2D>();
            _positionOffset = new Vector2(_rb.transform.position.x - _rbToFollow.transform.position.x, 0);
            _draggingDirection = _positionOffset.normalized;
        }

        private void FixedUpdate()
        {
            if(_rbToFollow != null)
            {
                Vector2 rbToFollowDirection = _rbToFollow.velocity.normalized;
                //If we are pushing the object, the rb to follow might enter
                //in collision with the draggable rb, making the draggable following the
                //rb to Follow not working.
                //Therefor, we follow if we go in the opposite direction
                //Otherwise we add force on the draggable
                if (rbToFollowDirection == _draggingDirection)
                {
                    _rb.AddForce(_draggingDirection * _rb.mass/1.75f, ForceMode2D.Impulse);
                } else if(rbToFollowDirection.x != 0)
                {
                    _rb.MovePosition((Vector2)_rbToFollow.transform.position + _positionOffset);
                }

            }
        }
    }
}