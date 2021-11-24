using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Environment
{
    public class RespawnOnDestroy : MonoBehaviour
    {
        LevelEventManager _levelEventManager;
        Vector3 _initPosition;
        Quaternion _initRotation;
        Transform _initParent;

        // Start is called before the first frame update
        void Start()
        {
            _levelEventManager = LevelEventManager.current;
            _initPosition = transform.position;
            _initRotation = transform.rotation;
            _initParent = transform.parent;

            _levelEventManager.onDestroyGameObject += HandleOnDestroyGameObject;
        }

        private void OnDestroy()
        {
            _levelEventManager.onDestroyGameObject -= HandleOnDestroyGameObject;
        }

        void HandleOnDestroyGameObject(int gameObjectId)
        {
            if (gameObjectId == gameObject.GetInstanceID())
            {
                transform.position = _initPosition;
                transform.rotation = _initRotation;
                transform.parent = _initParent;
            }
        }
    }
}