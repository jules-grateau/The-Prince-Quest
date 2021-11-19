using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class DraggableController : MonoBehaviour
    {
        EventManager eventManager;
        // Use this for initialization
        void Awake()
        {
            eventManager = EventManager.current;
            eventManager.onStartInteractWith += HandleStartInteractWith;
            eventManager.onStopInteractWith += HandleStopInteractWith;
        }

        private void OnDestroy()
        {
            eventManager.onStartInteractWith -= HandleStartInteractWith;
            eventManager.onStopInteractWith -= HandleStopInteractWith;
        }

        void HandleStartInteractWith(int gameobjectId)
        {
            if(gameObject.GetInstanceID() == gameobjectId)
            {
                eventManager.StartDragging(gameObject);
            }
        }

        void HandleStopInteractWith(int gameobjectId)
        {
            if (gameObject.GetInstanceID() == gameobjectId)
            {
                eventManager.StopDragging(gameObject);
            }
        }
    }
}