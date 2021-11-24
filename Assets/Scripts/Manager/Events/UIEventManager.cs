using Assets.Scripts.Enum;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Manager.Events
{
    public class UIEventManager : MonoBehaviour
    {
        public static UIEventManager current;

        private void Awake()
        {
            current = this;
        }

        public event Action<ScreenType> onCloseScreen;
        public void CloseScreen(ScreenType screenType)
        {
            if (onCloseScreen != null)
            {
                onCloseScreen(screenType);
            }
        }

        public event Action<ScreenType> onOpenScreen;
        public void OpenScreen(ScreenType screenType)
        {
            if (onOpenScreen != null)
            {
                onOpenScreen(screenType);
            }
        }

        public event Action<ButtonType> onClickButton;
        public void ClickButton(ButtonType buttonType)
        {
            if (onClickButton != null)
            {
                onClickButton(buttonType);
            }
        }

        public event Action<ButtonType> onActivateButton;
        public void ActivateButton(ButtonType buttonType)
        {
            if (onActivateButton != null)
            {
                onActivateButton(buttonType);
            }
        }

        public event Action<UiTextElementType, string> onUpdateTextElement;
        public void UpdateTextElement(UiTextElementType elementType, string value)
        {
            if (onUpdateTextElement != null)
            {
                onUpdateTextElement(elementType, value);
            }
        }

        public event Action<TutorialMessage> onOpenTutorialMessage;
        public void OpenTutorialMessage(TutorialMessage tutorialMessage)
        {
            if (onOpenTutorialMessage != null)
            {
                onOpenTutorialMessage(tutorialMessage);
            }
        }

        public event Action onCloseTutorialMessage;
        public void CloseTutorialMessage()
        {
            if (onCloseTutorialMessage != null)
            {
                onCloseTutorialMessage();
            }
        }
    }
}