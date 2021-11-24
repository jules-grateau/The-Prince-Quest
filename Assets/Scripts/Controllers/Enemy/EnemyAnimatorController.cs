using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        private EventManager eventManager;
        private Animator animator;
        private int instanceId;
        private const string isDeadParameterName = "isDead";
        // Start is called before the first frame update
        void Start()
        {
            eventManager = EventManager.current;
            animator = GetComponent<Animator>();
            eventManager.onEnemyDie += setIsDeadParameterTrue;
            instanceId = gameObject.GetInstanceID();
        }

        private void OnDestroy()
        {
            eventManager.onEnemyDie -= setIsDeadParameterTrue;
        }

        void setIsDeadParameterTrue(int instanceId)
        {
            if (instanceId == this.instanceId)
            {
                animator.SetBool(isDeadParameterName, true);
            }
        }
    }
}