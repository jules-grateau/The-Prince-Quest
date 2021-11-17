using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class AddLifeTrigger : MonoBehaviour
    {
        EventManager eventManager;
        // Use this for initialization
        void Start()
        {
            eventManager = EventManager.current;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                eventManager.AddLife(collision.transform.position);
            }
        }

    }
}