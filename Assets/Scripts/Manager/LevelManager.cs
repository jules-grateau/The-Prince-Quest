using System.Collections.Generic;
using UnityEngine;

public enum LevelType
{
    LevelZeroOne,
    LevelZeroTwo,
    LevelZeroThree
}

public class Level
{
    public Level(string path, string name)
    {
        Path = path;
        Name = name;
    }
    public string Path { get; }
    public string Name { get;}

}

public class LevelManager : MonoBehaviour
{
    private readonly Dictionary<LevelType, Level> levelDictionary = new Dictionary<LevelType, Level>
    {
         { LevelType.LevelZeroOne, new Level("Level0-1","0-1")},
         { LevelType.LevelZeroTwo, new Level("Level0-2","0-2")},
         { LevelType.LevelZeroThree, new Level("Level0-3","0-3")}
    };

    private const string levelPrefabsPath = "Prefabs/Levels/";
    private GameObject currentLevel;
    private GameObject currentLevelInstance;
    private EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onLoadLevel += handleLoadLevel;
        eventManager.onReloadLevel += handleOnReloadLevel;
        eventManager.onUnloadLevel += handleUnloadLevel;
    }

    void handleUnloadLevel()
    {
        if(currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }
    }

    void handleOnReloadLevel()
    {
        if(currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = Instantiate(currentLevel);
        }
    }

    void handleLoadLevel(LevelType levelType)
    {
        Level level;
        if (!levelDictionary.TryGetValue(levelType, out level))
            return;

        if(currentLevelInstance != null )
        {
            Destroy(currentLevelInstance);
        }
        GameObject levelToLoad = Resources.Load<GameObject>(levelPrefabsPath + level.Path);
        currentLevel = levelToLoad;
        currentLevelInstance = Instantiate(levelToLoad);
        eventManager.UpdateTextElement(UiTextElementType.Level,level.Name);
    } 
}
