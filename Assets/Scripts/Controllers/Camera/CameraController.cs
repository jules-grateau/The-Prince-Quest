using Assets.Scripts.Controllers.Player;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Controllers.Camera
{
    public class CameraController : MonoBehaviour
    {
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
            Debug.Log("Loading camera");
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            Transform parent = transform.parent.transform;
            //Transform player = parent.Find(PlayerGameObjectPath);
            Transform player = parent.GetComponentInChildren<PlayerMouvementController>()?.gameObject.transform;

            if (player != null)
            {
                _virtualCamera.Follow = player;
            }
            else
            {
                Debug.Log($"Camera couldn't find player to follow");
            }

            _cinemachineConfiner = GetComponent<CinemachineConfiner>();
            Transform cameraBoundaries = parent.Find(CameraBoundariesGameObjectPath);
            if (cameraBoundaries != null)
            {
                _cinemachineConfiner.m_BoundingShape2D = cameraBoundaries.GetComponent<Collider2D>();
            } else
            {
                Debug.Log($"Camera couldn't find boudaries : {CameraBoundariesGameObjectPath}");
            }

        }
    }
}