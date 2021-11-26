using Assets.Scripts.Manager.Events;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerStatus
    {
        public int HealthPoint { get; set; }

        public static PlayerStatus DefaultPlayerStatus = new PlayerStatus
        {
            HealthPoint = 2
        };

        public PlayerStatus Clone()
        {
            return new PlayerStatus { HealthPoint = HealthPoint };
        }
    }
    public class PlayerStatusController : MonoBehaviour
    {
        PlayerEventManager _playerEventManager;
        EnemyEventManager _enemyEventManager;
        InputEventManager _inputEventManager;

        bool _isInvulnerable = false;
        bool _isAlive = true;
        int _canInteractWith;

        public float invulnerabilityTime = 1f;
        PlayerStatus _playerStatus;

        // Start is called before the first frame update
        void Awake()
        {
            _enemyEventManager = EnemyEventManager.current;
            _enemyEventManager.onEnemyCollidedWithPlayer += HandleEnemyCollidedWithPlayer;

            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onKillPlayer += HandleKillPlayer;
            _playerEventManager.onCanInteractWith += HandleCanInteractWith;
            _playerEventManager.onSetPlayerStatus += HandleSetPlayerStatus;

            _inputEventManager = InputEventManager.current;
            _inputEventManager.onInteractKeyDown += HandleInteractKeyDown;
            _inputEventManager.onInteractKeyUp += HandleInteractKeyUp;
        }
        void OnDestroy()
        {
            _enemyEventManager.onEnemyCollidedWithPlayer -= HandleEnemyCollidedWithPlayer;

            _playerEventManager.onKillPlayer -= HandleKillPlayer;
            _playerEventManager.onCanInteractWith -= HandleCanInteractWith;
            _playerEventManager.onSetPlayerStatus -= HandleSetPlayerStatus;

            _inputEventManager.onInteractKeyDown -= HandleInteractKeyDown;
            _inputEventManager.onInteractKeyUp -= HandleInteractKeyUp;
        }

        void HandleSetPlayerStatus(PlayerStatus playerStatus)
        {
            if(playerStatus != null)
            {
                this._playerStatus = playerStatus;
            } else
            {
                this._playerStatus = PlayerStatus.DefaultPlayerStatus.Clone();
                
            }
            UpdateLife();
        }

        void HandleCanInteractWith(int gameobjectId)
        {
            if (gameobjectId == 0 && _canInteractWith != 0)
            {
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
            if (!_isAlive || _isInvulnerable)
                return;

            TakeDamage();
            if (_playerStatus.HealthPoint <= 0 )
            {
                PlayerDie();
            } else
            {
                StartCoroutine(Invulnerability());
            }
        }

        void TakeDamage()
        {
            _playerStatus.HealthPoint--;

            UpdateLife();
        }

        IEnumerator Invulnerability()
        {
            UpdateInvulnebirility(true);
            yield return new WaitForSeconds(invulnerabilityTime);
            UpdateInvulnebirility(false);
        }

        void UpdateInvulnebirility(bool isInvulnerable)
        {
            _playerEventManager.PlayerInvulnerability(isInvulnerable);
            _isInvulnerable = isInvulnerable;
        }

        void UpdateLife()
        {
            _playerEventManager.UpdatePlayerStatus(_playerStatus);
        }
    }
}