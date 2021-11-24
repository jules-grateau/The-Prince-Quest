using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    public class AddScoreTrigger : MonoBehaviour
    {
        public bool destroyOnTrigger = true;
        public int scoreToAdd = 200;

        private GameStateEventManager _gameStateEventManager;
        // Start is called before the first frame update
        void Start()
        {
            _gameStateEventManager = GameStateEventManager.current;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _gameStateEventManager.AddScore(transform.position, scoreToAdd);
                if (destroyOnTrigger)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}