using Assets.Scripts.Manager.Events;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyCollisionController : MonoBehaviour
    {
        private EnemyEventManager _enemyEventManager;
        private PlayerEventManager _playerEventManager;
        private CompositeCollider2D _compositeCollider;
        private int _instanceId;
        private bool _isAlive = true;
        private bool _cantCollide;

        // Start is called before the first frame update
        void Start()
        {
            _enemyEventManager = EnemyEventManager.current;
            _compositeCollider = GetComponent<CompositeCollider2D>();
            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onPlayerSteppedOnEnemy += HandlePlayerSteppedOnEnemy;
            _instanceId = gameObject.GetInstanceID();
            _enemyEventManager.onEnemyDie += HandleEnemyDie;
        }

        private void OnDestroy()
        {
            _playerEventManager.onPlayerSteppedOnEnemy -= HandlePlayerSteppedOnEnemy;
            _enemyEventManager.onEnemyDie -= HandleEnemyDie;
        }



        void HandlePlayerSteppedOnEnemy(int instanceId)
        {
            if (instanceId == this._instanceId)
            {
                StartCoroutine(DisableCollision());
            }
        }

        IEnumerator DisableCollision()
        {
            _cantCollide = true;
            yield return new WaitForSeconds(2);
            _cantCollide = false;
        }

        void HandleEnemyDie(int instanceId)
        {
            if (instanceId == this._instanceId)
            {
                _isAlive = false;
                _compositeCollider.enabled = false;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_isAlive || _cantCollide)
                return;

            GameObject objectCollided = collision.gameObject;
            if (objectCollided.CompareTag("Player"))
            {
                Vector2 collisionDirection = objectCollided.transform.position - transform.position;
                _enemyEventManager.EnemyCollidedWithPlayer(collisionDirection.normalized);
            }
        }
    }
}