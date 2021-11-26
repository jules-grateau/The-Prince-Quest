using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class DoorController : MonoBehaviour
    {
        public LevelType levelToLoad;

        PlayerEventManager _playerEventManager;
        bool _isPlayerDead = false;
        bool _triggered = false;

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
            _isPlayerDead = true;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isPlayerDead || _triggered)
                return;


            if (collision.CompareTag("Player"))
            {
                LevelEventManager.current.DoorEnter(levelToLoad);
                _triggered = true;
            }
        }
    }
}