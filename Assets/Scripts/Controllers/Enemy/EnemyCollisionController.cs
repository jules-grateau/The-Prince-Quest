using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyCollisionController : MonoBehaviour
    {
        private EnemyEventManager _enemyEventManager;
        private CompositeCollider2D _compositeCollider;
        private int _instanceId;
        private bool _isAlive = true;
        // Start is called before the first frame update
        void Start()
        {
            _enemyEventManager = EnemyEventManager.current;
            _compositeCollider = GetComponent<CompositeCollider2D>();
            _instanceId = gameObject.GetInstanceID();
            _enemyEventManager.onEnemyDie += handleEnemyDie;
        }

        private void OnDestroy()
        {
            _enemyEventManager.onEnemyDie -= handleEnemyDie;
        }


        void handleEnemyDie(int instanceId)
        {
            if (instanceId == this._instanceId)
            {
                _isAlive = false;
                _compositeCollider.enabled = false;
            }
        }
        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_isAlive)
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