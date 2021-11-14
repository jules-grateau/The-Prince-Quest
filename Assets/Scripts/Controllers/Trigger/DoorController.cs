using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Level levelToLoad;
    private EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        eventManager.DoorEnter(levelToLoad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
