using Assets.Scripts.Enum;
using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerStartGameEvent : MonoBehaviour
    {
        public InGameEventType gameEvent;
        EventManager eventManager;

        public void Awake()
        {
            eventManager = EventManager.current;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                eventManager.StartGameEvent(gameEvent);
                gameObject.SetActive(false);
            }
        }
    }
}