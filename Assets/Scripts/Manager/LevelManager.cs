using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    class Level
    {
        public Level(string path, string name)
        {
            Path = path;
            Name = name;
        }
        public string Path { get; }
        public string Name { get; }

    }

    public class LevelManager : MonoBehaviour
    {
        private readonly Dictionary<LevelType, Level> levelDictionary = new Dictionary<LevelType, Level>
        {
            { LevelType.TestLevel, new Level ( "TestGround", "TEST")},
            { LevelType.LevelZeroOne, new Level("Level0-1","0-1")},
            { LevelType.LevelZeroTwo, new Level("Level0-2","0-2")},
            { LevelType.LevelZeroThree, new Level("Level0-3","0-3")},
            { LevelType.LevelZeroFour, new Level("Level0-4","0-4")},
            { LevelType.LevelOneOne, new Level("Level1-1","1-1")},
            { LevelType.LevelOneTwo, new Level("Level1-2","1-2")}
        };

        private const string levelPrefabsPath = "Prefabs/Levels/";
        private GameObject _currentLevel;
        private GameObject _currentLevelInstance;
        private LevelEventManager _levelEventManager;

        // Start is called before the first frame update
        void Start()
        {
            _levelEventManager = LevelEventManager.current;
            _levelEventManager.onLoadLevel += HandleLoadLevel;
            _levelEventManager.onReloadLevel += HandleOnReloadLevel;
            _levelEventManager.onUnloadLevel += HandleUnloadLevel;
        }

        void HandleUnloadLevel()
        {
            if (_currentLevelInstance != null)
            {
                Destroy(_currentLevelInstance);
                _currentLevelInstance = null;
            }
        }

        void HandleOnReloadLevel(Action callback)
        {
            if (_currentLevelInstance != null)
            {
                Destroy(_currentLevelInstance);
                _currentLevelInstance = Instantiate(_currentLevel);
                callback();
            }
        }

        void HandleLoadLevel(LevelType levelType, Action callback)
        {
            Level level;
            if (!levelDictionary.TryGetValue(levelType, out level))
                return;

            if (_currentLevelInstance != null)
            {
                Destroy(_currentLevelInstance);
            }
            GameObject levelToLoad = Resources.Load<GameObject>(levelPrefabsPath + level.Path);
            _currentLevel = levelToLoad;
            _currentLevelInstance = Instantiate(levelToLoad);
            UIEventManager.current.UpdateTextElement(UiTextElementType.Level, level.Name);
            callback();
        }
    }
}