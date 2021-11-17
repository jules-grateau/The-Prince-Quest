using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UiTextElementType
{
    Score,
    Level,
    Life
}
[RequireComponent(typeof(Text))]
public class UiTextElementController : MonoBehaviour
{
    public UiTextElementType type;
    EventManager eventManager;
    Text textElement;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onUpdateTextElement += HandleUpdateTextElement;
        textElement = GetComponent<Text>();
    }

    void HandleUpdateTextElement(UiTextElementType type, string value)
    {
        if(this.type == type)
        {
            textElement.text = value;
        }
    }

}
