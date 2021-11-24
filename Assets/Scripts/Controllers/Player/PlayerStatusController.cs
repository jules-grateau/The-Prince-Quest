using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerStatusController : MonoBehaviour
    {
        EventManager eventManager;
        bool isAlive = true;
        int canInteractWith;
        // Start is called before the first frame update
        void Start()
        {
            eventManager = EventManager.current;
            eventManager.onEnemyCollidedWithPlayer += HandleEnemyCollidedWithPlayer;
            eventManager.onKillPlayer += HandleKillPlayer;
            eventManager.onCanInteractWith += HandleCanInteractWith;
            eventManager.onInteractKeyDown += HandleInteractKeyDown;
            eventManager.onInteractKeyUp += HandleInteractKeyUp;
        }
        void OnDestroy()
        {
            eventManager.onEnemyCollidedWithPlayer -= HandleEnemyCollidedWithPlayer;
            eventManager.onKillPlayer -= HandleKillPlayer;
            eventManager.onCanInteractWith -= HandleCanInteractWith;
            eventManager.onInteractKeyDown -= HandleInteractKeyDown;
            eventManager.onInteractKeyUp -= HandleInteractKeyUp;
        }

        void HandleCanInteractWith(int gameobjectId)
        {
            if (gameobjectId == 0 && canInteractWith != 0)
            {
                Debug.Log("Can't interact with the object anymore");
                eventManager.StopInteractWith(canInteractWith);
            }

            canInteractWith = gameobjectId;
        }

        void HandleInteractKeyDown()
        {
            if (canInteractWith != 0)
            {
                eventManager.StartInteractWith(gameObject, canInteractWith);
            }
        }

        void HandleInteractKeyUp()
        {
            if (canInteractWith != 0)
            {
                eventManager.StopInteractWith(canInteractWith);
            }
        }

        void PlayerDie()
        {
            isAlive = false;
            eventManager.PlayerDie();
        }

        void HandleKillPlayer()
        {
            if (!isAlive)
                return;

            PlayerDie();

        }
        void HandleEnemyCollidedWithPlayer(Vector2 direction)
        {
            if (!isAlive)
                return;

            PlayerDie();
        }
    }
}