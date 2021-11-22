using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    Dictionary<TutorialMessage, string> tutorialMessageText = new Dictionary<TutorialMessage, string>
    {
        {TutorialMessage.MoveKey, "Use the \"A & D\" keys or the directionals arrow to move"},
        {TutorialMessage.JumpPress, "Use the \"Space\" key to jump" },
        {TutorialMessage.JumpPressLong, "Keep the \"Space\" key longer to jump higher" },
        {TutorialMessage.InteractableItems, "Press \"E\" to interact with objetcs" }
    };
    EventManager eventManager;
    Text tutorialText;
    private void Start()
    {
        tutorialText = GetComponentInChildren<Text>();
        eventManager = EventManager.current;
        eventManager.onOpenTutorialMessage += HandleOpenTutorialMessage;
        eventManager.onCloseTutorialMessage += HandleCloseTutorialMessage;

        gameObject.SetActive(false);
    }

    void HandleOpenTutorialMessage(TutorialMessage tutorialMessage)
    {
        string text;
        if(tutorialMessageText.TryGetValue(tutorialMessage, out text))
        {
            tutorialText.text = text;
            gameObject.SetActive(true);
        }
    }

    void HandleCloseTutorialMessage()
    {
        tutorialText.text = "";
        gameObject.SetActive(false);
    }
}
