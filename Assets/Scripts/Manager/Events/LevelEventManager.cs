using Assets.Scripts.Enum;
using System;
using UnityEngine;

namespace Assets.Scripts.Manager.Events
{
    public class LevelEventManager : MonoBehaviour
    {
        public static LevelEventManager current;

        private void Awake()
        {
            current = this;
        }

        public event Action<LevelType,Action> onLoadLevel;
        public void LoadLevel(LevelType levelType, Action callback)
        {
            if (onLoadLevel != null)
            {
                onLoadLevel(levelType, callback);
            }
        }

        public event Action<Action> onReloadLevel;
        public void ReloadLevel(Action callback)
        {
            if (onReloadLevel != null)
            {
                onReloadLevel(callback);
            }
        }

        public event Action onUnloadLevel;
        public void UnloadLevel()
        {
            if (onUnloadLevel != null)
            {
                onUnloadLevel();
            }
        }

        public event Action<InGameEventType> onStartGameEvent;
        public void StartGameEvent(InGameEventType gameEventType)
        {
            if (onStartGameEvent != null)
            {
                onStartGameEvent(gameEventType);
            }
        }

        public event Action<LevelType> onDoorEnter;
        public void DoorEnter(LevelType levelType)
        {
            if (onDoorEnter != null)
            {
                onDoorEnter(levelType);
            }
        }

        public event Action<int> onDestroyGameObject;
        public void DestroyGameObject(int gameObjectId)
        {
            if (onDestroyGameObject != null)
            {
                onDestroyGameObject(gameObjectId);
            }
        }
    }
}