using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class ScreenController : MonoBehaviour
    {
        public ScreenType screenType;
        private EventManager eventManager;
        private void Start()
        {
            eventManager = EventManager.current;
            eventManager.onOpenScreen += HandleOpenScreen;
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