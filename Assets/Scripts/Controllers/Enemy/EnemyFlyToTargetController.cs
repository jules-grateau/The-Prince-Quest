using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyFlyToTargetController : MonoBehaviour
    {

        public float smoothTime = 0.05f;
        public float maxSpeed = 1f;
        bool _foundTarget = false;

        GameObject _eyes;
        const string EyesGameObjectName = "Eyes";

        Vector2 _target;
        int _gameObjectId;
        EnemyEventManager _enemyEventManager;
        Vector2 _currDampVelocity;

        // Use this for initialization
        void Start()
        {
            _eyes = transform.Find(EyesGameObjectName)?.gameObject;
            _enemyEventManager = EnemyEventManager.current;
            _enemyEventManager.onEnemyFindTarget += HandleEnemyFindTarget;
            _gameObjectId = gameObject.GetInstanceID();
        }

        private void OnDestroy()
        {
            _enemyEventManager.onEnemyFindTarget -= HandleEnemyFindTarget;
        }

        // Update is called once per frame
        void Update()
        {
            if(_foundTarget)
            {
                transform.position = Vector2.SmoothDamp(transform.position, _target, ref _currDampVelocity, smoothTime, maxSpeed);
            }

        }

        void HandleEnemyFindTarget(int gameObjectId, Vector2 position)
        {
            if(gameObjectId == _gameObjectId)
            {
                _foundTarget = true;
                _target = position;
                if(!IsLookingTowardTarget())
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
                }
            }
        }

        bool IsLookingTowardTarget()
        {
            Vector2 currLookingDirection = new Vector2(_eyes.transform.position.x - transform.position.x,0).normalized;
            Vector2 targetDirection = new Vector2(_target.x - transform.position.x, 0).normalized;

            return currLookingDirection == targetDirection;
        }
    }
}