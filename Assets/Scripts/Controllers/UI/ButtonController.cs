using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonController : MonoBehaviour
    {
        Button _button;
        UIEventManager _uiEventManager;

        public ButtonType buttonType;

        void Start()
        {
            _uiEventManager = UIEventManager.current;
            _uiEventManager.onActivateButton += HandleActivateButton;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }
        private void OnDestroy()
        {
            _uiEventManager.onActivateButton -= HandleActivateButton;
        }

        public void HandleActivateButton(ButtonType buttonType)
        {
            if(this.buttonType == buttonType)
            {
                _button.interactable = true;
            }
        }

        private void OnButtonClicked()
        {
            _uiEventManager.ClickButton(buttonType);
        }
    }
}