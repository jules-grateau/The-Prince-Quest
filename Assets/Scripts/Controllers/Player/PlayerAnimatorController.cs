using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        PlayerEventManager _playerEventManager;
        Animator _animator;

        const string IsWalkingParameterName = "isWalking";
        const string IsGroundedParameterName = "isGrounded";
        const string IsAliveParameterName = "isAlive";
        const string IsKissingParameterName = "isKissing";
        const string IsDraggingParameterName = "isDragging";
        const string IsInvulnerableParameterName = "isInvulnerable";

        // Start is called before the first frame update
        void Start()
        {
            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onWalking += SetWalkingParameter;
            _playerEventManager.onGrounded += SetGroundedParameter;
            _playerEventManager.onPlayerDie += SetIsAliveParameterFalse;
            _playerEventManager.onIsKissing += SetKissingParameter;
            _playerEventManager.onStartDragging += SetDragParameterTrue;
            _playerEventManager.onStopDragging += SetDragParameterFalse;
            _playerEventManager.onPlayerInvulnerability += SetIsInvulnerableParameter;
            _animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            _playerEventManager.onWalking -= SetWalkingParameter;
            _playerEventManager.onGrounded -= SetGroundedParameter;
            _playerEventManager.onPlayerDie -= SetIsAliveParameterFalse;
            _playerEventManager.onIsKissing -= SetKissingParameter;
            _playerEventManager.onStartDragging -= SetDragParameterTrue;
            _playerEventManager.onStopDragging -= SetDragParameterFalse;
            _playerEventManager.onPlayerInvulnerability -= SetIsInvulnerableParameter;
        }


        void SetDragParameterTrue(int gameObjectId)
        {
            _animator.SetBool(IsDraggingParameterName, true);
        }

        void SetDragParameterFalse(int gameObjectId)
        {
            _animator.SetBool(IsDraggingParameterName, false);
        }

        void SetKissingParameter(bool isKissing)
        {
            _animator.SetBool(IsKissingParameterName, isKissing);
        }

        void SetIsAliveParameterFalse()
        {
            _animator.SetBool(IsAliveParameterName, false);
        }
        void SetWalkingParameter(bool isWalking)
        {
            _animator.SetBool(IsWalkingParameterName, isWalking);
        }

        void SetGroundedParameter(bool isGrounded)
        {
            _animator.SetBool(IsGroundedParameterName, isGrounded);
        }

        void SetIsInvulnerableParameter(bool isInvulnerable)
        {
            _animator.SetBool(IsInvulnerableParameterName,isInvulnerable);
        }
    }
}