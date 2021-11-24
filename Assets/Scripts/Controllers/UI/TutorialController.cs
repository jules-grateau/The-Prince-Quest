using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    UIEventManager _uiEventManager;
    Text _tutorialText;

    readonly Dictionary<TutorialMessage, string> tutorialMessageText = new Dictionary<TutorialMessage, string>
    {
        {TutorialMessage.MoveKey, "Use the \"A & D\" keys or the directionals arrow to move"},
        {TutorialMessage.JumpPress, "Use the \"Space\" key to jump" },
        {TutorialMessage.JumpPressLong, "Keep the \"Space\" key longer to jump higher" },
        {TutorialMessage.InteractableItems, "Press \"E\" to interact with objetcs" }
    };

    private void Start()
    {
        _tutorialText = GetComponentInChildren<Text>();
        _uiEventManager = UIEventManager.current;
        _uiEventManager.onOpenTutorialMessage += HandleOpenTutorialMessage;
        _uiEventManager.onCloseTutorialMessage += HandleCloseTutorialMessage;

        gameObject.SetActive(false);
    }

    void HandleOpenTutorialMessage(TutorialMessage tutorialMessage)
    {
        string text;
        if(tutorialMessageText.TryGetValue(tutorialMessage, out text))
        {
            _tutorialText.text = text;
            gameObject.SetActive(true);
        }
    }

    void HandleCloseTutorialMessage()
    {
        _tutorialText.text = "";
        gameObject.SetActive(false);
    }
}
