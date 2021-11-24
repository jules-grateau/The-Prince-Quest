using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Controllers.Environment
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DraggableController : MonoBehaviour
    {
        EventManager eventManager;
        Rigidbody2D rb;
        Rigidbody2D rbToFollow;
        Vector2 positionOffset;
        Vector2 draggingDirection;
        // Use this for initialization
        void Awake()
        {
            eventManager = EventManager.current;
            rb = GetComponent<Rigidbody2D>();
            eventManager.onStartInteractWith += HandleStartInteractWith;
            eventManager.onStopInteractWith += HandleStopInteractWith;
        }

        private void OnDestroy()
        {
            eventManager.onStartInteractWith -= HandleStartInteractWith;
            eventManager.onStopInteractWith -= HandleStopInteractWith;
        }

        void HandleStartInteractWith(GameObject interactionFrom, int gameobjectId)
        {
            if(gameObject.GetInstanceID() == gameobjectId)
            {
                StartDragging(interactionFrom);
                eventManager.StartDragging(gameobjectId);
            }
        }

        void HandleStopInteractWith(int gameobjectId)
        {
            if (gameObject.GetInstanceID() == gameobjectId)
            {
                rbToFollow = null;
                eventManager.StopDragging(gameobjectId);
            }
        }

        void StartDragging(GameObject interactionFrom)
        {
            rbToFollow = interactionFrom.GetComponent<Rigidbody2D>();
            positionOffset = new Vector2(rb.transform.position.x - rbToFollow.transform.position.x, 0);
            draggingDirection = positionOffset.normalized;
        }

        private void FixedUpdate()
        {
            if(rbToFollow != null)
            {
                Vector2 rbToFollowDirection = rbToFollow.velocity.normalized;
                //If we are pushing the object, the rb to follow might enter
                //in collision with the draggable rb, making the draggable following the
                //rb to Follow not working.
                //Therefor, we follow if we go in the opposite direction
                //Otherwise we add force on the draggable
                if (rbToFollowDirection == draggingDirection)
                {
                    rb.AddForce(draggingDirection * rb.mass/1.75f, ForceMode2D.Impulse);
                } else if(rbToFollowDirection.x != 0)
                {
                    rb.MovePosition((Vector2)rbToFollow.transform.position + positionOffset);
                }

            }
        }
    }
}