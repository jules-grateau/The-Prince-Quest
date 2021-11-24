using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class AddLifeTrigger : MonoBehaviour
    {
        PlayerEventManager _playerEventManager;
        // Use this for initialization
        void Start()
        {
            _playerEventManager = PlayerEventManager.current;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                _playerEventManager.AddLife(collision.transform.position);
            }
        }

    }
}