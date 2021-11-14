using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    CinemachineConfiner cinemachineConfiner;
    EventManager eventManager;
    // Start is called before the first frame update

    void Awake()
    {
        Debug.Log("Awake camera n"+gameObject.GetInstanceID());
        LoadCamera();
    }


    void LoadCamera()
    {
        eventManager = EventManager.current;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        GameObject player = GameObject.Find("Player");
        if (player != null)
            virtualCamera.Follow = player.transform;
        else
            Debug.Log(" camera n" + gameObject.GetInstanceID() + " could find player");

        cinemachineConfiner = GetComponent<CinemachineConfiner>();
        GameObject cameraBoundaries = GameObject.Find("CameraBoundaries");
        if (cameraBoundaries != null)
        {
            cinemachineConfiner.m_BoundingShape2D = cameraBoundaries.GetComponent<Collider2D>();
        }
        else
            Debug.Log(" camera n" + gameObject.GetInstanceID() + " could find boundaries");
    }

    private void OnDestroy()
    {
        Debug.Log("Destroying camera N" + gameObject.GetInstanceID());
    }
}
