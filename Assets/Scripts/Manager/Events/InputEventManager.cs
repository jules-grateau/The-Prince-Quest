using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Manager.Events
{
    public class InputEventManager : MonoBehaviour
    {
        public static InputEventManager current;

        private void Awake()
        {
            current = this;
        }

        public event Action<float> onHorizontalInput;
        public void HorizontalInput(float horizontalInput)
        {
            if (onHorizontalInput != null)
            {
                onHorizontalInput(horizontalInput);
            }
        }

        public event Action onSpaceInputDown;
        public void SpaceInputDown()
        {
            if (onSpaceInputDown != null)
            {
                onSpaceInputDown();
            }
        }

        public event Action onSpaceInput;
        public void SpaceInput()
        {
            if (onSpaceInput != null)
            {
                onSpaceInput();
            }
        }

        public event Action onSpaceInputUp;
        public void SpaceInputUp()
        {
            if (onSpaceInputUp != null)
            {
                onSpaceInputUp();
            }
        }



        public event Action onEscapeInput;
        public void EscapeInput()
        {
            if (onEscapeInput != null)
            {
                onEscapeInput();
            }
        }

        public event Action<bool> onStopPlayerInput;
        public void StopPlayerInput(bool shouldStop)
        {
            if (onStopPlayerInput != null)
            {
                onStopPlayerInput(shouldStop);
            }
        }

        public event Action onInteractKeyDown;
        public void InteractKeyDown()
        {
            if (onInteractKeyDown != null)
            {
                onInteractKeyDown();
            }
        }

        public event Action onInteractKeyUp;
        public void InteractKeyUp()
        {
            if (onInteractKeyUp != null)
            {
                onInteractKeyUp();
            }
        }
    }
}