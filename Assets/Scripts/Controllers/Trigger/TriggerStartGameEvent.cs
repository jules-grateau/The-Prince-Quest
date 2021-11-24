using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerStartGameEvent : MonoBehaviour
    {
        public InGameEventType gameEvent;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                LevelEventManager.current.StartGameEvent(gameEvent);
                gameObject.SetActive(false);
            }
        }
    }
}