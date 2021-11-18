using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.UI
{


    [RequireComponent(typeof(Button))]
    public class ButtonController : MonoBehaviour
    {
        private Button button;
        private EventManager eventManager;
        public ButtonType buttonType;

        void Start()
        {
            eventManager = EventManager.current;
            eventManager.onActivateButton += HandleActivateButton;
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }
        private void OnDestroy()
        {
            eventManager.onActivateButton -= HandleActivateButton;
        }

        public void HandleActivateButton(ButtonType buttonType)
        {
            if(this.buttonType == buttonType)
            {
                button.interactable = true;
            }
        }

        private void OnButtonClicked()
        {
            eventManager.ClickButton(buttonType);
        }
    }
}