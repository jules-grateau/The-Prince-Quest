using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Controllers.Camera
{
    public class CameraController : MonoBehaviour
    {
        public const string PlayerGameObjectPath = "Player";
        public const string CameraBoundariesGameObjectPath = "CameraRail/CameraBoundaries";
        CinemachineVirtualCamera _virtualCamera;
        CinemachineConfiner _cinemachineConfiner;
        // Start is called before the first frame update

        void Awake()
        {
            LoadCamera();
        }


        void LoadCamera()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            Transform parent = transform.parent.transform;
            Transform player = parent.Find(PlayerGameObjectPath);

            if (player != null)
            {
                _virtualCamera.Follow = player;
            }
            else
            {
                Debug.Log($"Couldn't find player at path : {PlayerGameObjectPath}");
            }

            _cinemachineConfiner = GetComponent<CinemachineConfiner>();
            Transform cameraBoundaries = parent.Find(CameraBoundariesGameObjectPath);
            if (cameraBoundaries != null)
            {
                _cinemachineConfiner.m_BoundingShape2D = cameraBoundaries.GetComponent<Collider2D>();
            }

        }
    }
}