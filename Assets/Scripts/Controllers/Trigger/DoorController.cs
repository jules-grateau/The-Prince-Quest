using Assets.Scripts.Enum;
using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class DoorController : MonoBehaviour
    {
        public LevelType levelToLoad;
        private EventManager eventManager;
        private bool isPlayerDead = false;
        // Start is called before the first frame update
        void Start()
        {
            eventManager = EventManager.current;
            eventManager.onPlayerDie += handlePlayerDie;
        }

        private void OnDestroy()
        {
            eventManager.onPlayerDie -= handlePlayerDie;
        }

        void handlePlayerDie()
        {
            isPlayerDead = true;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (isPlayerDead)
                return;


            if (collision.CompareTag("Player"))
            {
                eventManager.DoorEnter(levelToLoad);
            }
        }
    }
}