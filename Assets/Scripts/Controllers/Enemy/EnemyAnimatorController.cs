using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        private EnemyEventManager _enemyEventManager;
        private Animator _animator;
        private int _instanceId;
        private const string IsDeadParameterName = "isDead";

        // Start is called before the first frame update
        void Start()
        {
            _enemyEventManager = EnemyEventManager.current;
            _animator = GetComponent<Animator>();
            _enemyEventManager.onEnemyDie += setIsDeadParameterTrue;
            _instanceId = gameObject.GetInstanceID();
        }

        private void OnDestroy()
        {
            _enemyEventManager.onEnemyDie -= setIsDeadParameterTrue;
        }

        void setIsDeadParameterTrue(int instanceId)
        {
            if (instanceId == this._instanceId)
            {
                _animator.SetBool(IsDeadParameterName, true);
            }
        }
    }
}