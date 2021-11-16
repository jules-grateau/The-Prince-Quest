using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreTrigger : MonoBehaviour
{
    public bool destroyOnTrigger = true;
    public int scoreToAdd = 200;
    private EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventManager.AddScore(transform.position, scoreToAdd);
            if(destroyOnTrigger)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
