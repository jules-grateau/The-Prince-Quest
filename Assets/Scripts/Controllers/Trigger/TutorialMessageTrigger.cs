using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class TutorialMessageTrigger : MonoBehaviour
    {
        public TutorialMessage tutorialMessage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                UIEventManager.current.OpenTutorialMessage(tutorialMessage);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                UIEventManager.current.CloseTutorialMessage();
            }
        }
    }
}