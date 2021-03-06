using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerCollisionController : MonoBehaviour
    {
        PlayerEventManager _playerEventManager;
        Rigidbody2D _playerRb;
        BoxCollider2D _boxCollider;
        bool _isAlive = true;
        bool _isInvulnerable = false;
        bool _isDragging = false;
        GameObject _lookDirectionObject;
        GameObject _feet;

        public float groundCollisionWidthMultiplier = 0.95f;
        public float groundCollisionHeight = 0.10f;
        public float interacteDistance = 0.5f;
        public LayerMask groundLayer;
        public LayerMask enemyLayer;

        const string FeetGameObjectName = "Feet";
        const string LookDirectionGameObjectName = "LookDirection";

        // Start is called before the first frame update
        void Start()
        {
            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onPlayerDie += HandlePlayerDie;
            _playerEventManager.onStartDragging += HandleStartDragging;
            _playerEventManager.onStopDragging += HandleStopDragging;
            _playerEventManager.onPlayerInvulnerability += HandlePlayerInvulnerability;
            _boxCollider = GetComponent<BoxCollider2D>();
            _playerRb = GetComponent<Rigidbody2D>();
            _lookDirectionObject = transform.Find(LookDirectionGameObjectName)?.gameObject;
            _feet = transform.Find(FeetGameObjectName)?.gameObject;
        }

        private void OnDestroy()
        {
            _playerEventManager.onPlayerDie -= HandlePlayerDie;
            _playerEventManager.onStartDragging -= HandleStartDragging;
            _playerEventManager.onStartDragging -= HandleStopDragging;
            _playerEventManager.onPlayerInvulnerability -= HandlePlayerInvulnerability;
        }

        void Update()
        {
            if (!_isAlive)
                return;

            CalculateInteractable();
        }

        private void FixedUpdate()
        {
            if (!_isAlive)
                return;

            CalculateGroundColision();
            CalculateEnemyColision();
            CalculateIsCrushed();
        }

        void CalculateInteractable()
        {
            Vector2 direction = _lookDirectionObject.transform.position.x - transform.position.x < 0 ? Vector2.left : Vector2.right;
            float actualInteractDistance = interacteDistance;
            if (_isDragging)
            {
                actualInteractDistance *= 2;
            }
            RaycastHit2D interactableOverlapRay = Physics2D.Raycast(transform.position, direction, actualInteractDistance, groundLayer);
            Collider2D collider = interactableOverlapRay.collider;

            if (collider != null && collider.CompareTag("Interactable"))
            {
                _playerEventManager.CanInteractWith(collider.gameObject.GetInstanceID());
            }
            else
            {
                _playerEventManager.CanInteractWith(0);
            }

            Debug.DrawRay(transform.position, direction * actualInteractDistance, collider != null && collider.CompareTag("Interactable") ? Color.green : Color.red);
        }

        void CalculateGroundColision()
        {
            Vector2 boxPoint = _feet.transform.position;
            Vector2 boxSize = new Vector2(_boxCollider.size.x, groundCollisionHeight);
            // Collider to detected collision with ground, to check if grounded
            Collider2D groundOverlapBox = Physics2D.OverlapBox(boxPoint
                , boxSize, transform.rotation.y, groundLayer);
            bool groundHit = groundOverlapBox != null;
            Debug.DrawRay(boxPoint, boxSize, groundHit ? Color.green : Color.red);
            _playerEventManager.Grounded(groundHit);
        }

        void CalculateEnemyColision()
        {
            if (_isInvulnerable)
                return;

            // Collider to detected collision with ground, to check if grounded
            Collider2D enemyOverlapBox = Physics2D.OverlapCircle(_feet.transform.position,
                _boxCollider.size.x, enemyLayer);
            Debug.DrawRay(_feet.transform.position, Vector2.right * _boxCollider.size.x);
            Debug.DrawRay(_feet.transform.position, Vector2.left * _boxCollider.size.x);
            Debug.DrawRay(_feet.transform.position, Vector2.up * _boxCollider.size.x);
            Debug.DrawRay(_feet.transform.position, Vector2.down * _boxCollider.size.x);
            if (enemyOverlapBox != null)
            {
                _playerEventManager.PlayerSteppedOnEnemy(enemyOverlapBox.gameObject.GetInstanceID());
            }
        }

        void CalculateIsCrushed()
        {
            Collider2D[] overlapBox = Physics2D.OverlapBoxAll(_playerRb.position + _boxCollider.offset, _boxCollider.size, 0);
            bool isPushedLeft = false;
            bool isPushedRight = false;

            foreach (Collider2D collider in overlapBox)
            {
                if (collider.isTrigger)
                    return;

                var colliderPosition = collider.ClosestPoint(transform.position);
                if (colliderPosition.x < transform.position.x)
                {
                    isPushedLeft = true;
                }
                if (colliderPosition.x > transform.position.x)
                {
                    isPushedRight = true;
                }
            }


            if (isPushedLeft && isPushedRight)
            {
                _playerEventManager.KillPlayer();
            }
        }

        void HandlePlayerDie()
        {
            _isAlive = false;
        }

        void HandleStartDragging(int goId)
        {
            _isDragging = true;
        }

        void HandleStopDragging(int goId)
        {
            _isDragging = false;
        }

        void HandlePlayerInvulnerability(bool isInvulnerable)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer,  LayerMask.NameToLayer("Enemies"), isInvulnerable);
            _isInvulnerable = isInvulnerable;
        }
    }
}