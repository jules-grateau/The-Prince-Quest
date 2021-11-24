using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class DoorController : MonoBehaviour
    {
        public LevelType levelToLoad;

        private PlayerEventManager _playerEventManager;
        private bool isPlayerDead = false;
        // Start is called before the first frame update
        void Start()
        {
            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onPlayerDie += handlePlayerDie;
        }

        private void OnDestroy()
        {
            _playerEventManager.onPlayerDie -= handlePlayerDie;
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
                LevelEventManager.current.DoorEnter(levelToLoad);
            }
        }
    }
}