using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class AnimationManager : MonoBehaviour
    {
        AnimationEventManager _animationEventManager;
        // Use this for initialization
        void Start()
        {
            _animationEventManager = AnimationEventManager.current;
            _animationEventManager.onStartAnimation += HandleStartAnimation;
            _animationEventManager.onStopAnimation += HandleStopAnimation;
        }

        void HandleStartAnimation(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.KanaKissPlayer:
                    InputEventManager.current.StopPlayerInput(true);
                    break;
            }
        }

        void HandleStopAnimation(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.KanaKissPlayer:
                    InputEventManager.current.StopPlayerInput(false);
                    break;
            }
        }
    }
}