using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyMouvementController : MonoBehaviour
    {
        public float speed = 250f;
        public float xMinDistance;
        public float xMaxDistance;
        public float deathSpeed = 3f;

        private EnemyEventManager _enemyEventManager;
        private GameObject _lookDirectionObject;
        private float _xFirstDestination;
        private float _xSecondDestination;
        private float _xDestination;
        private Vector2 _destinationDirection;
        private Rigidbody2D _enemyRb;
        private bool _isAlive = true;
        private int _instanceId;

        // Start is called before the first frame update
        void Start()
        {
            _enemyEventManager = EnemyEventManager.current;
            _enemyRb = GetComponent<Rigidbody2D>();
            _instanceId = gameObject.GetInstanceID();
            _enemyEventManager.onEnemyDie += HandleEnemyDie;
            _lookDirectionObject = transform.GetChild(0).gameObject;
            _xFirstDestination = transform.position.x + xMinDistance;
            _xSecondDestination = transform.position.x + xMaxDistance;
            _xDestination = _xFirstDestination;
            CalculateDestinationDirection();
            CalculateLookDirection();
        }

        private void OnDestroy()
        {
            _enemyEventManager.onEnemyDie -= HandleEnemyDie;
        }
        void HandleEnemyDie(int instanceId)
        {
            if (instanceId == this._instanceId)
            {
                _enemyRb.isKinematic = true;
                _enemyRb.velocity = Vector2.zero;
                _isAlive = false;
                GetComponent<Collider2D>().isTrigger = true;
            }
        }

        void CalculateDestinationDirection()
        {
            var destinationDistance = _xDestination - transform.position.x;

            if (destinationDistance > 0)
                _destinationDirection = Vector2.right;
            if (destinationDistance < 0)
                _destinationDirection = Vector2.left;
        }

        void CalculateLookDirection()
        {
            Vector2 lookDirection = _lookDirectionObject.transform.position - transform.position;

            if (_destinationDirection == Vector2.right && lookDirection.x < 0)
            {
                _enemyRb.transform.eulerAngles = _enemyRb.transform.eulerAngles + new Vector3(0, 180, 0);
            }
            if (_destinationDirection == Vector2.left && lookDirection.x > 0)
            {
                _enemyRb.transform.eulerAngles = _enemyRb.transform.eulerAngles + new Vector3(0, 180, 0);
            }
        }

        void SetNextDestination()
        {
            if (_xDestination == _xFirstDestination)
            {
                _xDestination = _xSecondDestination;
            }
            else
            {
                _xDestination = _xFirstDestination;
            }
            CalculateDestinationDirection();
            CalculateLookDirection();
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (_isAlive)
            {
                _enemyRb.velocity = new Vector2(speed * _destinationDirection.x, _enemyRb.velocity.y);

                if (_destinationDirection == Vector2.right && _xDestination <= transform.position.x)
                    SetNextDestination();

                if (_destinationDirection == Vector2.left && _xDestination >= transform.position.x)
                    SetNextDestination();

                return;
            }

            if (!_isAlive)
            {
                transform.Translate(Vector2.down * deathSpeed * Time.deltaTime);
            }

        }
    }
}