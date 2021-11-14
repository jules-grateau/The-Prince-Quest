using UnityEngine;

public enum Level
{
    LevelZero,
    LevelOne
}
public class LevelManager : MonoBehaviour
{
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
    }

    void handleOnReloadLevel()
    {
        if(currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = Instantiate(currentLevel);
            eventManager.ResumeGame();
        }
    }

    void handleLoadLevel(Level level)
    {
        if(currentLevelInstance != null )
        {
            Destroy(currentLevelInstance);
        }
        GameObject levelToLoad = Resources.Load<GameObject>(levelPrefabsPath + level.ToString());
        currentLevel = levelToLoad;
        currentLevelInstance = Instantiate(levelToLoad);
    } 
}
