using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public enum AnimationType
    {
        LevelZero_KanaKiss
    }
    public class AnimationManager : MonoBehaviour
    {
        EventManager eventManager;
        // Use this for initialization
        void Start()
        {
            eventManager = EventManager.current;
            eventManager.onStartAnimation += HandleStartAnimation;
            eventManager.onStopAnimation += HandleStopAnimation;
        }

        void HandleStartAnimation(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.LevelZero_KanaKiss:
                    eventManager.StopPlayerInput(true);
                    break;
            }
        }

        void HandleStopAnimation(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.LevelZero_KanaKiss:
                    eventManager.StopPlayerInput(false);
                    break;
            }
        }
    }
}