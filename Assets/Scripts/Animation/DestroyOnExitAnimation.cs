using UnityEngine;

namespace Assets.Scripts.Animation
{
    public class DestroyOnExitAnimation : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject);
        }
    }
}