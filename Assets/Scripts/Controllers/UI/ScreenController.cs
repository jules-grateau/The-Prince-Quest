using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class ScreenController : MonoBehaviour
    {
        public ScreenType screenType;
        private UIEventManager _uiEventManager;
        private void Start()
        {
            _uiEventManager = UIEventManager.current;
            _uiEventManager.onOpenScreen += HandleOpenScreen;
        }

        private void OnDestroy()
        {
            _uiEventManager.onOpenScreen -= HandleOpenScreen;
        }

        void HandleOpenScreen(ScreenType screenType)
        {
            if(this.screenType == screenType)
            {
                gameObject.SetActive(true);
            } else
            {
                gameObject.SetActive(false);
            }
        }

    }
}