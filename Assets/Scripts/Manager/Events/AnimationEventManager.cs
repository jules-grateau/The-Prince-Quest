using Assets.Scripts.Enum;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Manager.Events
{
    public class AnimationEventManager : MonoBehaviour
    {
        public static AnimationEventManager current;

        private void Awake()
        {
            current = this;
        }

        public event Action<AnimationType> onStartAnimation;
        public void StartAnimation(AnimationType animationType)
        {
            if (onStartAnimation != null)
            {
                onStartAnimation(animationType);
            }
        }

        public event Action<AnimationType> onStopAnimation;
        public void StopAnimation(AnimationType animationType)
        {
            if (onStopAnimation != null)
            {
                onStopAnimation(animationType);
            }
        }

        public event Action<AnimationType> onTriggerAnimation;
        public void TriggerAnimation(AnimationType animationType)
        {
            if (onTriggerAnimation != null)
            {
                onTriggerAnimation(animationType);
            }
        }
    }
}