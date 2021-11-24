using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class KillZoneController : MonoBehaviour
    {
        EventManager eventManager;
        // Start is called before the first frame update
        void Start()
        {
            eventManager = EventManager.current;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                eventManager.KillPlayer();
            }
        }
    }
}