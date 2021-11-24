using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyStatusController : MonoBehaviour
    {
        private EnemyEventManager _enemyEventManager;
        private PlayerEventManager _playerEventManager;
        
        private int _instanceId;
        private bool _isAlive = true;

        public int scoreValue = 50;
        
        // Start is called before the first frame update
        void Start()
        {
            _enemyEventManager = EnemyEventManager.current;
            _playerEventManager = PlayerEventManager.current;

            _playerEventManager.onPlayerSteppedOnEnemy += HandlePlayerSteppedOnEnemy;
            _enemyEventManager.onEnemyDie += HandleEnemyDie;
            
            _instanceId = gameObject.GetInstanceID();
        }

        private void OnDestroy()
        {
            _enemyEventManager.onEnemyDie -= HandleEnemyDie;
        }

        void HandlePlayerSteppedOnEnemy(int instanceId)
        {
            if (_isAlive && instanceId == this._instanceId)
            {
                _enemyEventManager.EnemyDie(this._instanceId);
                Destroy(gameObject, 2);
            }
        }

        void HandleEnemyDie(int instanceId)
        {
            if (instanceId == this._instanceId)
            {
                GameStateEventManager.current.AddScore(transform.position, scoreValue);
                _isAlive = false;
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}