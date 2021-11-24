using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UiTextElementController : MonoBehaviour
{
    public UiTextElementType type;

    UIEventManager uiEventManager;
    Text _textElement;

    // Start is called before the first frame update
    void Start()
    {
        uiEventManager = UIEventManager.current;
        uiEventManager.onUpdateTextElement += HandleUpdateTextElement;
        _textElement = GetComponent<Text>();
    }

    void HandleUpdateTextElement(UiTextElementType type, string value)
    {
        if(this.type == type)
        {
            _textElement.text = value;
        }
    }

}
