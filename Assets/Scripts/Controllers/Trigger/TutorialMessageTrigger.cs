using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class TutorialMessageTrigger : MonoBehaviour
    {
        public TutorialMessage tutorialMessage;
        EventManager eventManager;

        private void Awake()
        {
            eventManager = EventManager.current;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                eventManager.OpenTutorialMessage(tutorialMessage);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                eventManager.CloseTutorialMessage();
            }
        }
    }
}