using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Trigger
{
    [RequireComponent(typeof(Collider2D))]
    public class DestroyBoundary : MonoBehaviour
    {
        LevelEventManager _levelEventManager;

        private void Awake()
        {
            _levelEventManager = LevelEventManager.current;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            _levelEventManager.DestroyGameObject(collision.gameObject.GetInstanceID());
        }
    }
}