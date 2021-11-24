using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DestroyBoundary : MonoBehaviour
{
    EventManager eventManager;

    private void Awake()
    {
        eventManager = EventManager.current;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        eventManager.DestroyGameObject(collision.gameObject.GetInstanceID());
    }
}
