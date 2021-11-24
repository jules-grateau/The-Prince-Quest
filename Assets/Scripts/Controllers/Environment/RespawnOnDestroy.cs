using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Controllers.Environment
{
    public class RespawnOnDestroy : MonoBehaviour
    {
        EventManager eventManager;
        Vector3 initPosition;
        Quaternion initRotation;
        Transform initParent;
        // Start is called before the first frame update
        void Start()
        {
            eventManager = EventManager.current;
            initPosition = transform.position;
            initRotation = transform.rotation;
            initParent = transform.parent;

            eventManager.onDestroyGameObject += HandleOnDestroyGameObject;
        }

        private void OnDestroy()
        {
            eventManager.onDestroyGameObject -= HandleOnDestroyGameObject;
        }

        void HandleOnDestroyGameObject(int gameObjectId)
        {
            if (gameObjectId == gameObject.GetInstanceID())
            {
                transform.position = initPosition;
                transform.rotation = initRotation;
                transform.parent = initParent;
            }
        }
    }
}