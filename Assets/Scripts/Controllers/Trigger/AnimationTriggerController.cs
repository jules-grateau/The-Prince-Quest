using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class AnimationTriggerController : MonoBehaviour
    {
        public AnimationType animationType;
        public bool isConsumable = true;
        
        PlayerEventManager _playerEventManager;

        bool _isConsumed = false;
        bool _isPlayerDead = false;
        
        // Start is called before the first frame update
        void Start()
        {
            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onPlayerDie += HandlePlayerDie;
        }

        void OnDestroy()
        {
            _playerEventManager.onPlayerDie -= HandlePlayerDie;
        }

        void HandlePlayerDie()
        {
            _isPlayerDead = true;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isPlayerDead)
                return;
            if (isConsumable && _isConsumed)
                return;

            if (collision.CompareTag("Player"))
            {
                AnimationEventManager.current.TriggerAnimation(animationType);
                _isConsumed = true;
            }
        }
    }
}