using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerStatusController : MonoBehaviour
    {
        PlayerEventManager _playerEventManager;
        EnemyEventManager _enemyEventManager;
        InputEventManager _inputEventManager;

        bool _isAlive = true;
        int _canInteractWith;

        // Start is called before the first frame update
        void Start()
        {
            _enemyEventManager = EnemyEventManager.current;
            _enemyEventManager.onEnemyCollidedWithPlayer += HandleEnemyCollidedWithPlayer;

            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onKillPlayer += HandleKillPlayer;
            _playerEventManager.onCanInteractWith += HandleCanInteractWith;

            _inputEventManager = InputEventManager.current;
            _inputEventManager.onInteractKeyDown += HandleInteractKeyDown;
            _inputEventManager.onInteractKeyUp += HandleInteractKeyUp;
        }
        void OnDestroy()
        {
            _enemyEventManager.onEnemyCollidedWithPlayer -= HandleEnemyCollidedWithPlayer;

            _playerEventManager.onKillPlayer -= HandleKillPlayer;
            _playerEventManager.onCanInteractWith -= HandleCanInteractWith;

            _inputEventManager.onInteractKeyDown -= HandleInteractKeyDown;
            _inputEventManager.onInteractKeyUp -= HandleInteractKeyUp;
        }

        void HandleCanInteractWith(int gameobjectId)
        {
            if (gameobjectId == 0 && _canInteractWith != 0)
            {
                Debug.Log("Can't interact with the object anymore");
                _playerEventManager.StopInteractWith(_canInteractWith);
            }

            _canInteractWith = gameobjectId;
        }

        void HandleInteractKeyDown()
        {
            if (_canInteractWith != 0)
            {
                _playerEventManager.StartInteractWith(gameObject, _canInteractWith);
            }
        }

        void HandleInteractKeyUp()
        {
            if (_canInteractWith != 0)
            {
                _playerEventManager.StopInteractWith(_canInteractWith);
            }
        }

        void PlayerDie()
        {
            _isAlive = false;
            _playerEventManager.PlayerDie();
        }

        void HandleKillPlayer()
        {
            if (!_isAlive)
                return;

            PlayerDie();

        }
        void HandleEnemyCollidedWithPlayer(Vector2 direction)
        {
            if (!_isAlive)
                return;

            PlayerDie();
        }
    }
}