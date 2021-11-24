using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Controllers.Camera
{
    public class CameraController : MonoBehaviour
    {
        public string PlayerGameObjectPath = "Player";
        public string CameraBoundariesGameObjectPath = "CameraRail/CameraBoundaries";
        CinemachineVirtualCamera virtualCamera;
        CinemachineConfiner cinemachineConfiner;
        // Start is called before the first frame update

        void Awake()
        {
            LoadCamera();
        }


        void LoadCamera()
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            Transform parent = transform.parent.transform;
            Transform player = parent.Find(PlayerGameObjectPath);

            if (player != null)
            {
                virtualCamera.Follow = player;
            }
            else
            {
                Debug.Log($"Couldn't find player at path : {PlayerGameObjectPath}");
            }

            cinemachineConfiner = GetComponent<CinemachineConfiner>();
            Transform cameraBoundaries = parent.Find(CameraBoundariesGameObjectPath);
            if (cameraBoundaries != null)
            {
                cinemachineConfiner.m_BoundingShape2D = cameraBoundaries.GetComponent<Collider2D>();
            }

        }
    }
}